import numpy as np
from scipy.signal import butter, lfilter, sosfiltfilt
import time
import os, glob
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.externals import joblib
import shutil
from datetime import datetime
import socket

def Re_referencing(eegData, channelNum, sampleNum):
        after_car = np.zeros((channelNum,sampleNum))
        for i in np.arange(channelNum):
            after_car[i,:] = eegData[i,:] - np.mean(eegData,axis=0)
        return after_car

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
    
def Epoching(eegData, stims, code, samplingRate, nChannel, epochSampleNum, epochOffset,baseline):
        Time = stims[np.where(stims[:,1] == code),0][0]
        Time = np.floor(np.multiply(Time,samplingRate)).astype(int)
        Time_after = np.add(Time,epochOffset).astype(int)
        Time_base = np.add(Time,baseline).astype(int)
        Num = Time.shape
        Epochs = np.zeros((Num[0], nChannel, epochSampleNum))
        for j in range(Num[0]):
            Epochs[j, :, :] = eegData[:, Time_after[j]:Time_after[j] + epochSampleNum]
            for i in range(nChannel):
                Epochs[j, i, :] = np.subtract(Epochs[j, i, :], np.mean(eegData[i,Time_after[j]:Time_base[j]]))
        return [Epochs,Num[0]]

def Online_Convert_to_featureVector2(Epochs, Num, featureNum):
    Features = np.zeros((Num, featureNum))
    for i in range(Num):
        Features[i,:] = np.reshape(Epochs[i,:,:], (1, featureNum))
    return Features


def main():
        #load cnn model and predict result
#         model = load_model('C:/Users/wldk5/WorldSystem/Zero/ZeroModel/ZeroCNN2.h5')
#        global file_exist, file1, file2, channelNum
        eegData_txt = 'C:/Users/NTH417/Desktop/Drone/LDA/data/eegData.out'
        stims_txt = 'C:/Users/NTH417/Desktop/Drone/LDA/data/stims.out'
        start_txt = 'C:/Users/NTH417/Desktop/Drone/LDA/data/start.out'
        moveData_eeg = 'C:/Users/NTH417/Desktop/Drone/LDA/Online/Data/txt_files/eegData/'
        moveData_stims = 'C:/Users/NTH417/Desktop/Drone/LDA/Online/Data/txt_files/stims/'
        
        Classifier_path = "C:/Users/NTH417/Desktop/Drone/LDA/Model/"
        current_list2 = []
        current_list2 = sorted(glob.glob(Classifier_path + '*.pickle'), key=os.path.getmtime, reverse=True)
        Classifier_real = current_list2[0]
        lda = LinearDiscriminantAnalysis(solver='lsqr',shrinkage='auto')
        lda = joblib.load(Classifier_real)
        
        serverSock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        serverSock.bind(('', 12240))
        serverSock.listen(0)
        connectionSock, addr = serverSock.accept()
        print(str(addr),'에서 접속이 확인되었습니다.')
        
        for i in range(0, 12):
            #load text file
            while True:
                if os.path.isfile(start_txt):
                    break
            start_time = time.time()
            
            while(time.time() - start_time < 25):
                pass
            
            while True:
                if os.path.isfile(eegData_txt) & os.path.isfile(stims_txt):
                    processing_time = time.time()
                    os.remove(start_txt)
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
            
            ### Preprocessing process            
            sampleNum = eegData.shape[1]
            
            #Common Average Reference
            eegData = Re_referencing(eegData, channelNum, sampleNum)

            #Bandpass Filter
            eegData = butter_bandpass_filter(eegData, 0.5, 10, samplingFreq, 4)

            
            #Epoching
            epochSampleNum = int(np.floor(0.4 * samplingFreq))
            offset = int(np.floor(0.2 * samplingFreq)) # no delay 기준 0.2 - 0.6
            baseline = int(np.floor(0.6 * samplingFreq)) # delay 있으면 0.3 - 0.7으로 바꿔야함
            
            [Epochs1, Num1] = Epoching(eegData, stims, 1, samplingFreq, channelNum, epochSampleNum, offset, baseline)
            [Epochs2, Num2] = Epoching(eegData, stims, 2, samplingFreq, channelNum, epochSampleNum, offset, baseline)
            [Epochs3, Num3] = Epoching(eegData, stims, 3, samplingFreq, channelNum, epochSampleNum, offset, baseline)
            [Epochs4, Num4] = Epoching(eegData, stims, 4, samplingFreq, channelNum, epochSampleNum, offset, baseline)
            [Epochs5, Num5] = Epoching(eegData, stims, 5, samplingFreq, channelNum, epochSampleNum, offset, baseline)
            [Epochs6, Num6] = Epoching(eegData, stims, 6, samplingFreq, channelNum, epochSampleNum, offset, baseline)
            [Epochs7, Num7] = Epoching(eegData, stims, 7, samplingFreq, channelNum, epochSampleNum, offset, baseline)

            result = np.zeros((1,7))
            featureNum = channelNum*epochSampleNum
            
            Epochs1 = Online_Convert_to_featureVector2(Epochs1, Num1, featureNum)
            Epochs2 = Online_Convert_to_featureVector2(Epochs2, Num1, featureNum)
            Epochs3 = Online_Convert_to_featureVector2(Epochs3, Num1, featureNum)
            Epochs4 = Online_Convert_to_featureVector2(Epochs4, Num1, featureNum)
            Epochs5 = Online_Convert_to_featureVector2(Epochs5, Num1, featureNum)
            Epochs6 = Online_Convert_to_featureVector2(Epochs6, Num1, featureNum)
            Epochs6 = Online_Convert_to_featureVector2(Epochs7, Num1, featureNum)

            a1 = lda.predict(Epochs1)
            a2 = lda.predict(Epochs2)
            a3 = lda.predict(Epochs3)
            a4 = lda.predict(Epochs4)
            a5 = lda.predict(Epochs5)
            a6 = lda.predict(Epochs6)
            a7 = lda.predict(Epochs7)

            result[0,0] = np.sum(a1)
            result[0,1] = np.sum(a2)
            result[0,2] = np.sum(a3)
            result[0,3] = np.sum(a4)
            result[0,4] = np.sum(a5)
            result[0,5] = np.sum(a6)
            result[0,6] = np.sum(a7)

            answer = np.argmax(result) + 1
            
#            np.savetxt(result_txt, answer)
            print("Process time: ", time.time() - processing_time)
            print("Result: ", answer)
            connectionSock.send(str(answer).encode("utf-8"))
            
if __name__ == "__main__":
    main()

