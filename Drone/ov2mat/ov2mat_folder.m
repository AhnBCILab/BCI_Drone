myFolder = 'C:\Users\NTH417\Desktop\sion0714';
filePattern = fullfile(myFolder, '*.ov'); % Change to whatever pattern you need.
theFiles = dir(filePattern);
for k = 1 : length(theFiles)
    ovFileName = theFiles(k).name;
    matFileName = ovFileName(1:end-3);
    matFileName = strcat(matFileName, ".mat");
    fprintf(1, "%s \n", matFileName);
    convert_ov2mat(ovFileName, matFileName);
end