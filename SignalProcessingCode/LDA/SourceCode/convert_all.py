import matlab.engine
import os, glob
import shutil


def func(file_Path):
    ov_movePath = file_Path + "ov\\"
    mat_movePath = file_Path + "mat\\"
    current_list = sorted(glob.glob(file_Path + '*.ov'), key=os.path.getmtime)
    
    eng = matlab.engine.start_matlab()
    for ovfile in current_list:
        print(ovfile)
        ovhead, ovtail = os.path.split(ovfile)
        
        matfile_name = ovfile[:-3] + ".mat"
        mathead, mattail = os.path.split(matfile_name)
        
        k = eng.convert_ov2mat(ovfile, matfile_name)
        
        shutil.move(ovfile, ov_movePath + ovtail)
        shutil.move(matfile_name, mat_movePath + mattail)
  
def main():
    for i in range(1, 21):      
        if i < 10:
            sub = "S0" + str(i)
        else:
            sub = "S" + str(i)
        
        # Train_Path = "C:\\Users\\user\\Desktop\\DataBackup\\" + sub + "\\Training\\"
        # Online_Path = "C:\\Users\\user\\Desktop\\DataBackup\\" + sub + "\\Online\\"
        # func(Train_Path)
        # func(Online_Path)
        
        online_path = "C:\\Users\\user\\Desktop\\DataBackup\\" + sub + "\\Online\\mat\\"
        online_list = sorted(glob.glob(online_path + '*.mat'), key=os.path.getmtime)
        txt_path = "C:\\Users\\user\\Desktop\\DataBackup\\" + sub + "\\"
        txt_list = sorted(glob.glob(txt_path + '*.txt'), key=os.path.getmtime, reverse=True)
        
        result_file = open(txt_list[0], 'r', encoding = 'utf-16')
        lines = result_file.readlines()
        
        n = len(lines)
        print('trial : ', n)
        if i % 2 == 0:
            for j in range(0, int(n/2)):
                line = lines[j]
                mat_file = online_list[j]
                
                index = [int(s) for s in line.split() if s.isdigit()]
                
                path, filename = os.path.split(mat_file)
                newname = path + '\\'+ str(j+1) + '_' + str(index[1]) + '.mat'
                os.rename(mat_file, newname)
            
        else:
            for j in range(int(n/2), n):
                line = lines[j]
                mat_file = online_list[j]
                
                index = [int(s) for s in line.split() if s.isdigit()]
                
                path, filename = os.path.split(mat_file)
                newname = path + '\\'+ str(j+1) + '_' + str(index[1]) + '.mat'
                os.rename(mat_file, newname)
        
        print(sub + ' done')
        
if __name__ == "__main__":
    main()