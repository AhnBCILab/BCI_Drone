import numpy as np
from scipy.signal import butter, lfilter
from scipy import signal
import os, time
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from datetime import datetime
from sklearn.externals import joblib
import shutil

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
        Num = Time.shape
        Epochs = np.zeros((Num[0], nChannel, epochSampleNum))
        for j in range(Num[0]):
            Epochs[j, :, :] = eegData[:, Time_after[j]:Time_after[j] + epochSampleNum]
            
            #Baseline Correction
            for i in range(nChannel):
                Epochs[j, i, :] = Epochs[j, i, :] - np.mean(Epochs[j,i,:])
            
        return [Epochs, Num[0]]

def Convert_to_featureVector(EpochsT, NumT, EpochsN, NumN, featureNum):
        FeaturesT = np.zeros((NumT, featureNum))
        for i in range(NumT):
            FeaturesT[i,:] = np.reshape(EpochsT[i,:,:],(1,featureNum))
        FeaturesN = np.zeros((NumN, featureNum))
        for j in range(NumN):
            FeaturesN[j,:] = np.reshape(EpochsN[j,:,:],(1,featureNum))
        return [FeaturesT,FeaturesN]
    
# def Standardization(Epochs):
#     for i in range(Epochs.shape[1]):
#         Epochs[:,i,:] = np.subtract(Epochs[:,i,:], np.mean(Epochs[:,i,:]))
#         Epochs[:,i,:] = Epochs[:,i,:] / np.std(Epochs[:,i,:])
    
#     return Epochs

def Make_Average_Component(EpochsT, NumT, EpochsN, NumN, channelNum, epochSampleNum, componentNum):
    # EpochsT = Standardization(EpochsT)
    # EpochsN = Standardization(EpochsN)
    
    NumT_Aver = NumT-componentNum
    NumN_Aver = NumN-componentNum
    
    EpochsT_Aver = np.zeros((NumT_Aver, channelNum, epochSampleNum))
    EpochsN_Aver = np.zeros((NumN_Aver, channelNum, epochSampleNum))
    for i in range(NumT_Aver):
        EpochsT_Aver[i, :, :] = np.mean(EpochsT[i:i+componentNum, :, :], axis=0)
    for j in range(NumN_Aver):
        EpochsN_Aver[j, :, :] = np.mean(EpochsN[j:j+componentNum, :, :], axis=0)
        
    return [EpochsT_Aver, NumT_Aver, EpochsN_Aver, NumN_Aver]

def resampling(Epochs, EpochNum, resampleRate, channelNum):
        resampled_epoch = np.zeros((EpochNum, channelNum, resampleRate))
        for i in range(EpochNum):
            for j in range(channelNum):
                resampled_epoch[i,j,:] = signal.resample(Epochs[i,j,:], resampleRate)
        return resampled_epoch

def main():
        start = time.time()
        
        Data_path ="C:\\Users\\user\\Desktop\\Drone\\LDA\\Data\\"
        eegData_txt = Data_path + 'eegData.out'
        stims_txt = Data_path + 'stims.out'
        moveData_eeg = 'C:\\Users\\user\\Desktop\\Drone\\LDA\\Training\\eegData\\'
        moveData_stims = 'C:\\Users\\user\\Desktop\\Drone\\LDA\\Training\\stims\\'
        ##Generate Preprocessing Training data
        ctime = datetime.today().strftime("%m%d_%H%M")
        Classifier_path = 'C:/Users/user/Desktop/Drone/LDA/Model/' + ctime + 'Classifier.pickle'
        channelNum = 7
        samplingFreq = 300
   
        while True:
            if os.path.isfile(eegData_txt) & os.path.isfile(stims_txt):
                eegData = np.loadtxt(eegData_txt, delimiter = ",")
                stims = np.loadtxt(stims_txt, delimiter = ",")
                ctime = datetime.today().strftime("%m%d_%H%M%S")
                moveData_e = moveData_eeg + ctime + 'eegData.out'
                moveData_s = moveData_stims + ctime + 'stims.out'
                shutil.move(eegData_txt, moveData_e)
                shutil.move(stims_txt, moveData_s)
                break
                
        print("got process")
        
        ##Preprocessing process

        #Bandpass Filter
        eegData = butter_bandpass_filter(eegData, 0.1, 30, samplingFreq, order=4)
    
        #Epoching
        epochSampleNum = int(np.floor(1.0 * samplingFreq))
        offset = int(np.floor(0.0 * samplingFreq)) 
        baseline = int(np.floor(1.0 * samplingFreq)) 
        [EpochsT, NumT] = Epoching(eegData, stims, 1, samplingFreq, channelNum, epochSampleNum, offset, baseline)
        [EpochsN, NumN] = Epoching(eegData, stims, 0, samplingFreq, channelNum, epochSampleNum, offset, baseline)
        
        # EpochsN_New = np.zeros((NumT, channelNum, epochSampleNum))
        # NumN = NumT
        # for i in range(NumN):
        #     EpochsN_New[i,:,:] = np.mean(EpochsN[i*5:i*5+5, :, :], axis=0)
        
        resampleRate = 100
        
        #Convert to feature vector
        [EpochsT_Aver, NumT_Aver, EpochsN_Aver, NumN_Aver] = Make_Average_Component(EpochsT, NumT, EpochsN, NumN, channelNum, epochSampleNum, 20)
        EpochsT_Aver = resampling(EpochsT_Aver, NumT_Aver, resampleRate, channelNum) 
        EpochsN_Aver = resampling(EpochsN_Aver, NumN_Aver, resampleRate, channelNum)
        
        featureNum = channelNum*resampleRate
        [FeaturesT, FeaturesN] = Convert_to_featureVector(EpochsT_Aver, NumT_Aver, EpochsN_Aver, NumN_Aver, featureNum)
        TrainData = np.concatenate((FeaturesT, FeaturesN))
        TrainLabel = np.concatenate((np.ones((NumT_Aver,1)).astype(int),np.zeros((NumN_Aver,1)).astype(int))).ravel()
        
        #Saving LDA classifier
        lda = LinearDiscriminantAnalysis(solver='lsqr',shrinkage='auto')
        lda.fit(TrainData, TrainLabel)
        joblib.dump(lda, Classifier_path, protocol=2)
        
        print("time :", time.time() - start)
        
if __name__ == "__main__":
    main()