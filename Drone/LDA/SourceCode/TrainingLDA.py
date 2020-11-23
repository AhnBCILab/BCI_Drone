import numpy as np
import pickle
import hdf5storage
from scipy.signal import butter, lfilter
import os, glob, time
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
import matlab.engine
from datetime import datetime
from sklearn.externals import joblib
import random    

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
            
        return [Epochs, Num[0]]

def Convert_to_featureVector(EpochsT, NumT, EpochsN, NumN, featureNum):
        FeaturesT = np.zeros((NumT, featureNum))
        for i in range(NumT):
            FeaturesT[i,:] = np.reshape(EpochsT[i,:,:],(1,featureNum))
        FeaturesN = np.zeros((NumN, featureNum))
        for j in range(NumN):
            FeaturesN[j,:] = np.reshape(EpochsN[j,:,:],(1,featureNum))
        return [FeaturesT,FeaturesN]

def Balancing_DataSet(Epochs, size):
    Epochs_New = np.zeros((size, Epochs.shape[1], Epochs.shape[2]))
    
    index = np.random.choice(Epochs.shape[0], size = size, replace = False)
    
    Epochs_New = Epochs[index, :, :]
    
    return Epochs_New
    
def Standardization(Epochs):
    for i in range(Epochs.shape[1]):
        Epochs[:,i,:] = np.subtract(Epochs[:,i,:], np.mean(Epochs[:,i,:]))
        Epochs[:,i,:] = Epochs[:,i,:] / np.std(Epochs[:,i,:])
    
    return Epochs

def Make_Average_Component(EpochsT, NumT, EpochsN, NumN, channelNum, epochSampleNum, componentNum):
    EpochsT = Standardization(EpochsT)
    EpochsN = Standardization(EpochsN)
    
    NumT_Aver = NumT-componentNum
    NumN_Aver = NumN-componentNum
    
    EpochsT_Aver = np.zeros((NumT_Aver, channelNum, epochSampleNum))
    EpochsN_Aver = np.zeros((NumN_Aver, channelNum, epochSampleNum))
    for i in range(NumT_Aver):
        EpochsT_Aver[i, :, :] = np.mean(EpochsT[i:i+componentNum, :, :], axis=0)
    for j in range(NumN_Aver):
        EpochsN_Aver[j, :, :] = np.mean(EpochsN[j:j+componentNum, :, :], axis=0)
        
    return [EpochsT_Aver, NumT_Aver, EpochsN_Aver, NumN_Aver]


def main():
        start = time.time()
        
        ##Generate Preprocessing Training data
        ctime = datetime.today().strftime("%m%d_%H%M")
        Classifier_path = 'C:/Users/user/Desktop/Drone/LDA/Model/' + ctime + 'Classifier.pickle'
        channelNum = 7
        
        ov_Path = "C:\\Users\\user\\Desktop\\Drone\\LDA\\Training\\"
        current_list = sorted(glob.glob(ov_Path + '*.ov'), key=os.path.getmtime, reverse=True)
        ovfile_name = current_list[0]
        matfile_name = current_list[0][:-3] + ".mat"
    
        print("current ov file path:", current_list[0])
        eng = matlab.engine.start_matlab()
        k = eng.convert_ov2mat(ovfile_name, matfile_name)
        mat = hdf5storage.loadmat(matfile_name)
        channelNames = mat['channelNames']
        eegData = mat['eegData']
        samplingFreq = mat['samplingFreq']
        samplingFreq = samplingFreq[0,0]
        stims = mat['stims']
        channelNum = channelNames.shape
        channelNum = channelNum[1]
        eegData = np.transpose(eegData)
        
        ##Preprocessing process

        #Bandpass Filter
        eegData = butter_bandpass_filter(eegData, 0.23, 30, samplingFreq, order=4)
    
        #Epoching
        epochSampleNum = int(np.floor(1.0 * samplingFreq))
        offset = int(np.floor(0.0 * samplingFreq)) 
        baseline = int(np.floor(1.0 * samplingFreq)) 
        [EpochsT, NumT] = Epoching(eegData, stims, 1, samplingFreq, channelNum, epochSampleNum, offset, baseline)
        [EpochsN, NumN] = Epoching(eegData, stims, 0, samplingFreq, channelNum, epochSampleNum, offset, baseline)
        
        EpochsN_New = Balancing_DataSet(EpochsN, NumT)
        
        #Convert to feature vector
        [EpochsT_Aver, NumT_Aver, EpochsN_Aver, NumN_Aver] = Make_Average_Component(EpochsT, NumT, EpochsN_New, NumT, channelNum, epochSampleNum, 20)
        featureNum = channelNum*epochSampleNum
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
