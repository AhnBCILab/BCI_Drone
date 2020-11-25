import os, glob


for i in range(1, 21):
        if i < 10:
            sub = "S0" + str(i)
        else:
            sub = "S" + str(i)
    
        txt_path = "C:\\Users\\user\\Desktop\\DataBackup\\" + sub + "\\"
        txt_list = sorted(glob.glob(txt_path + '*.txt'), key=os.path.getmtime, reverse=True)
        
        result_file = open(txt_list[0], 'r', encoding = 'utf-16')
        lines = result_file.readlines()
        
        wrong_1 = 0
        wrong_2 = 0
        n = len(lines)
        print('lines : ' + str(n))
        for i in range(n):
            line = lines[i]
            
            index = [int(s) for s in line.split() if s.isdigit()]
            # print('index : ' + str(index[0]) + ' / ' + str(index[1]))
            
            if index[0] != index[1]:
                if i <= n/2-1:
                    wrong_1 += 1
                else:
                    wrong_2 += 1
        
        print("Subejct : " + sub)
        print("first trial : " + str(n/2 - wrong_1) + " / " + str(n/2))
        print("second trial : " + str(n/2 - wrong_2) + " / " +str(n/2))
            