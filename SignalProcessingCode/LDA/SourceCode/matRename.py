import os, glob

online_path = "C:\\Users\\user\\Desktop\\DataBackup\\S01\\Online\\mat\\"
online_list = sorted(glob.glob(online_path + '*.mat'), key=os.path.getmtime)
txt_path = "C:\\Users\\user\\Desktop\\DataBackup\\S01\\"
txt_list = sorted(glob.glob(txt_path + '*.txt'), key=os.path.getmtime, reverse=True)

result_file = open(txt_list[0], 'r', encoding = 'utf-16')
lines = result_file.readlines()

for i in range(len(lines)):
    line = lines[i]
    mat_file = online_list[i]
    
    index = [int(s) for s in line.split() if s.isdigit()]
    
    path, filename = os.path.split(mat_file)
    newname = path + '\\'+ str(i+1) + '_' + str(index[0]) + '.mat'
    os.rename(mat_file, newname)