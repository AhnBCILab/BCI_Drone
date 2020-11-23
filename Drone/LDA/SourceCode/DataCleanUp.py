import matlab.engine
import os, glob
import shutil

eng = matlab.engine.start_matlab()

def ov2mat(file_list, move_path):
    for ovfile in file_list:
        print(ovfile)
        ovhead, ovtail = os.path.split(ovfile)
        
        matfile_name = ovfile[:-3] + ".mat"
        mathead, mattail = os.path.split(matfile_name)
        
        k = eng.convert_ov2mat(ovfile, matfile_name)
        
        shutil.move(ovfile, move_path + 'ov\\' + ovtail)
        shutil.move(matfile_name, move_path + 'mat\\' + mattail)

def clean():
    desktop = 'C:\\Users\\user\\Desktop\\'
    train_path = 'C:\\Users\\user\\Desktop\\Drone\\LDA\\Training\\'
    online_path = 'C:\\Users\\user\\Desktop\\Drone\\LDA\\Online\\ov\\'
    txt_path = 'C:\\Users\\user\\Desktop\\BCILAB_DSI_DRONE_AR_VR\\DSI_Drone\\Assets\\StreamingAssets\\'
    txt_list = sorted(glob.glob(txt_path + '*.txt'), key=os.path.getmtime, reverse=True)
    train_list = sorted(glob.glob(train_path + '*.ov'), key=os.path.getmtime)
    online_list = sorted(glob.glob(online_path + '*.ov'), key=os.path.getmtime)
    
    # Create Folder
    txt_head, txt_tail = os.path.split(txt_list[0])
    folder_name = txt_tail[:-4]
    
    final_path = desktop + folder_name
    # os.mkdir(final_path)
    # os.mkdir(final_path + '\\Training')
    os.makedirs(final_path + '\\Training\\ov')
    os.mkdir(final_path + '\\Training\\mat')
    
    # os.mkdir(final_path + '\\Online')
    # os.mkdir(final_path + '\\Online\\VR')
    os.makedirs(final_path + '\\Online\\VR\\ov')
    os.mkdir(final_path + '\\Online\\VR\\mat')
    
    # os.mkdir(final_path + '\\Online\\AR')
    os.makedirs(final_path + '\\Online\\AR\\ov')
    os.mkdir(final_path + '\\Online\\AR\\mat')
    
    # Remove last ov file
    os.remove(online_list[-1])
    online_list = online_list[:-1]
    online_VR = online_list[:20]
    online_AR = online_list[20:]
    
    # Rename online mat files
    result_file = open(txt_list[0], 'r', encoding = 'utf-16')
    lines = result_file.readlines()
    # lines = lines[:-1]
    
    for i in range(20):
        line = lines[i]

        mat_file = online_VR[i]

        index = [int(s) for s in line.split() if s.isdigit()]
        
        path, filename = os.path.split(mat_file)
        
        newname = path + '\\'+ str(i+1) + '_' + str(index[0]) + '.mat'
        os.rename(mat_file, newname)
        
        online_VR[i] = newname
        
#         print('before:', mat_file)
#         print('after:', newname)
    
    # Convert Ov files to Mat files
    ov2mat(train_list, final_path + '\\Training\\')
    ov2mat(online_VR, final_path + '\\Online\\VR\\')
    
    for i in range(20):
        line = lines[i+20]

        mat_file = online_AR[i]

        index = [int(s) for s in line.split() if s.isdigit()]
        
        path, filename = os.path.split(mat_file)
        
        newname = path + '\\'+ str(i+1) + '_' + str(index[0]) + '.mat'
        os.rename(mat_file, newname)
        
        online_AR[i] = newname
    
    ov2mat(online_AR, final_path + '\\Online\\AR\\')
    
    # Move txt file
    shutil.move(txt_list[0], final_path + '\\' + txt_tail)
      
def main():
    clean()
    
if __name__ == "__main__":
    main()