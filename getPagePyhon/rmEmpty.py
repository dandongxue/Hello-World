#coding :utf-8
import os
__author__ = 'dandong'
def listdir(dir):
    fielnum =0
    list = os.listdir(dir)
    for line in list:
        filepath = os.path.join(dir,line)
        if os.path.isdir(filepath):
            if not os.path.exists(filepath+"\\1.gif"):
                print filepath.decode('gbk')
                os.rmdir(filepath)
                fielnum+=1

    print str(fielnum)
#  E:\Bayes\GitResp\Gitar-Play\getPagePyhon
listdir(r'E:\Bayes\GitResp\Gitar-Play\GitarPlay\Gitar')