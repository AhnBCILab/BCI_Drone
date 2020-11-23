import numpy as np
from scipy.signal import butter, lfilter
import time
import os, glob
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.externals import joblib
from scipy import signal
import shutil
from datetime import datetime
import socket

def butter_bandpass(lowcut, highcut, fs, order=5):
        nyq = 0.5 * fs
        low = lowcut / nyq
        high = highcut / nyq
        b, a = butter(order, [low, high], btype='band')
        return b, a
def butter_bandpass_filter(data, lowcut, highcut, fs, order=5):
        b, a = butter_bandpass(lowcut, highcut, fs, order=order)
        y = lfilter(b, a, data)
        return y

def Standardization(Epochs):
    for i in range(Epochs.shape[1]):
        Epochs[:,i,:] = np.subtract(Epochs[:,i,:], np.mean(Epochs[:,i,:]))
        Epochs[:,i,:] = Epochs[:,i,:] / np.std(Epochs[:,i,:])
    
    return Epochs      

def Epoching(eegData, stims, code, samplingRate, nChannel, epochSampleNum, epochOffset,baseline):
        Time = stims[np.where(stims[:,1] == code),0][0]
        Time = np.floor(np.multiply(Time,samplingRate)).astype(int)
        Time_after = np.add(Time,epochOffset).astype(int)
        Num = Time.shape
        Epochs = np.zeros((Num[0], nChannel, epochSampleNum))
        for j in range(Num[0]):
            Epochs[j, :, :] = eegData[:, Time_after[j]:Time_after[j] + epochSampleNum]
            
            # Baseline Correction
            for i in range(nChannel):
                Epochs[j,i,:] = Epochs[j,i,:] - np.mean(Epochs[j,i,:])
            
            
        # Epochs = Standardization(Epochs)
        Epochs_Aver = np.mean(Epochs, axis=0)

        return Epochs_Aver

def Convert_to_FeatureVector(Epochs, buttonNum, featureNum):
    Features = np.zeros((buttonNum, featureNum))
    for i in range(buttonNum):
        Features[i, :] = np.reshape(Epochs[i, :, :], (1, featureNum))
    return Features
    
def resampling(Epochs, resampleRate, channelNum):
        resampled_epoch = np.zeros((channelNum, resampleRate))
        for j in range(channelNum):
            resampled_epoch[j,:] = signal.resample(Epochs[j,:], resampleRate)
            
        return resampled_epoch    

def main():
#        global file_exist, file1, file2, channelNum
        Data_path ="C:\\Users\\user\\Desktop\\Drone\\LDA\\Data\\"
        eegData_txt = Data_path + 'eegData.out'
        stims_txt = Data_path + 'stims.out'
        moveData_eeg = 'C:\\Users\\user\\Desktop\\Drone\\LDA\\Online\\eegData\\'
        moveData_stims = 'C:\\Users\\user\\Desktop\\Drone\\LDA\\Online\\stims\\'
        
        Classifier_path = "C:\\Users\\user\\Desktop\\Drone\\LDA\\Model\\"
        
        current_list2 = sorted(glob.glob(Classifier_path + '*.pickle'), key=os.path.getmtime, reverse=True)
        Classifier_real = current_list2[0]
        lda = LinearDiscriminantAnalysis(solver='lsqr',shrinkage='auto')
        lda = joblib.load(Classifier_real)
        
        serverSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        serverSock.bind(('', 12240))
        serverSock.listen(0)
        connectionSock, addr = serverSock.accept()
        print(str(addr),'에서 접속이 확인되었습니다.')
        
        for i in range(0, 15):
            #load text file
            while True:
                if os.path.isfile(eegData_txt) & os.path.isfile(stims_txt):
                    processing_time = time.time()
                    eegData = np.loadtxt(eegData_txt, delimiter = ",")
                    stims = np.loadtxt(stims_txt, delimiter = ",")
                    ctime = datetime.today().strftime("%m%d_%H%M%S")
                    moveData_e = moveData_eeg + ctime + 'eegData.out'
                    moveData_s = moveData_stims + ctime + 'stims.out'
                    shutil.move(eegData_txt, moveData_e)
                    shutil.move(stims_txt, moveData_s)
                    break
                    
            print("got process")
            channelNum = 7
            samplingFreq = 300
            buttonNum = 7
            
            ### Preprocessing process            

            #Bandpass Filter
            eegData = butter_bandpass_filter(eegData, 0.1, 30, samplingFreq, 4)

                 #Epoching
            epochSampleNum = int(np.floor(1.0 * samplingFreq))
            offset = int(np.floor(0.0 * samplingFreq)) 
            baseline = int(np.floor(1.0 * samplingFreq)) 
            
            Epochs_Aver = np.zeros((buttonNum, channelNum, epochSampleNum))
            
            resampleRate = 100
            featureNum = channelNum*resampleRate
            
            Epochs_final = np.zeros((buttonNum, channelNum, resampleRate))
            
            for i in range(buttonNum):
                Epochs_Aver[i] = Epoching(eegData, stims, (i+1), samplingFreq, channelNum, epochSampleNum, offset, baseline)
                Epochs_final[i] = resampling(Epochs_Aver[i], resampleRate, channelNum)
                
            Features = Convert_to_FeatureVector(Epochs_final, buttonNum, featureNum)
            
            Answers = lda.decision_function(Features)
            answer = np.argmax(Answers) + 1
            
#            np.savetxt(result_txt, answer)
            print("Process time: ", time.time() - processing_time)
            print("Result: ", answer)
            connectionSock.send(str(answer).encode("utf-8"))
            
if __name__ == "__main__":
    main()