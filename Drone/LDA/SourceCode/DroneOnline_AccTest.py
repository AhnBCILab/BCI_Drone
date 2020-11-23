import numpy as np
from scipy.signal import butter, lfilter, sosfiltfilt
import time
import os, glob
import hdf5storage
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.externals import joblib
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

def Normalization(Epochs):
    for i in range(Epochs.shape[1]):
        Epochs[:,i,:] = np.subtract(Epochs[:,i,:], np.mean(Epochs[:,i,:]))
        Epochs[:,i,:] = Epochs[:,i,:] / np.std(Epochs[:,i,:])
    
    return Epochs    
    
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
        
        Epochs = Normalization(Epochs)
        Epochs_Aver = np.mean(Epochs, axis=0)
#         Epochs_Aver = Epochs[2]
        return Epochs_Aver

def Convert_to_FeatureVector(Epochs, buttonNum, featureNum):
    Features = np.zeros((buttonNum, featureNum))
    for i in range(buttonNum):
        Features[i, :] = np.reshape(Epochs[i, :, :], (1, featureNum))
    return Features
    
def main():
        Classifier = '/Users/hyuns/Desktop/0716_1326Classifier.pickle'

        lda = LinearDiscriminantAnalysis(solver='lsqr',shrinkage='auto')
        lda = joblib.load(Classifier)
        
        mat_path = '/Users/hyuns/Desktop/HGU/2020-2/Capstone/Drone Project/EEGData/VR300_Data/sion/Online/'
        current_list = []
        current_list = sorted(glob.glob(mat_path + '*.mat'), key=os.path.getmtime)
        
        for mat_file in current_list:
            ans = mat_file[-7]
            
            mat = hdf5storage.loadmat(mat_file)
            channelNames = mat['channelNames']
            eegData = mat['eegData']
            samplingFreq = mat['samplingFreq']
            samplingFreq = samplingFreq[0,0]
            stims = mat['stims']
            channelNum = channelNames.shape
            channelNum = channelNum[1]
            eegData = np.transpose(eegData)
            buttonNum = 6

            #Bandpass Filter
            eegData = butter_bandpass_filter(eegData, 0.2, 30, samplingFreq, 4)

            #Epoching
            epochSampleNum = int(np.floor(1.0 * samplingFreq))
            offset = int(np.floor(0.0 * samplingFreq)) 
            baseline = int(np.floor(1.0 * samplingFreq)) 

            Epochs_Aver = np.zeros((buttonNum, channelNum, epochSampleNum))
            featureNum = channelNum*epochSampleNum

            for i in range(buttonNum):
                    Epochs_Aver[i] = Epoching(eegData, stims, (i+1), samplingFreq, channelNum, epochSampleNum, offset, baseline)

            Features = Convert_to_FeatureVector(Epochs_Aver, buttonNum, featureNum)

            Answers = lda.predict(Features)
            answer = np.argmax(Answers) + 1

            print('order: ', ans, 'predict: ', answer)
        
if __name__ == "__main__":
    main()
