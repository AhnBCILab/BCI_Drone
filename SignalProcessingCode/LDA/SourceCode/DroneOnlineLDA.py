import numpy as np
import sys

def save_data(eegData, stims, eegData_txt, stims_txt):
        np.savetxt(eegData_txt, eegData, delimiter = ",")
        np.savetxt(stims_txt, stims, delimiter = ",")

class MyOVBox(OVBox):
    def __init__(self):
        OVBox.__init__(self)
        self.signalHeader = None
    
    def initialize(self):
            print('Python initialize function started')
            global eegData, stims, eegData_txt, stims_txt
            eegData = np.zeros((7,1))
            stims = np.zeros((1,3))
            
            Data_path = "C:\\Users\\user\\Desktop\\Drone\\LDA\\Data\\"  
            eegData_txt = Data_path + 'eegData.out'
            stims_txt = Data_path + 'stims.out'
    
    def process(self):
        global eegData, stims, eegData_txt, stims_txt
            #Signal acquisition
        for chunkIndex in range( len(self.input[0]) ):
            if(type(self.input[0][chunkIndex]) == OVSignalHeader):
                self.signalHeader = self.input[0].pop()                
            elif(type(self.input[0][chunkIndex]) == OVSignalBuffer):
                chunk = self.input[0].pop()
                signalRaw = np.array(chunk).reshape(tuple(self.signalHeader.dimensionSizes))
                eegData = np.append(eegData,signalRaw,axis=1)
            #Stimulation acquisition
            for chunkIndex in range( len(self.input[1]) ):
                chunk = self.input[1].pop()
                if(type(chunk) == OVStimulationSet):
                    for stimIdx in range(len(chunk)):
                        stim=chunk.pop();
                        x = np.array([[stim.date,stim.identifier,0.0]])
                        stims = np.append(stims, x, axis=0)
                            
    def uninitialize(self):
            global eegData, stims, eegData_txt, stims_txt
            print('Python uninitialize function started')
            print('got here')
            stims = np.delete(stims,0,0)
            save_data(eegData, stims, eegData_txt, stims_txt)
            return

box = MyOVBox()
	
