__author__ = 'dandong'
#coding:utf-8
import urllib2,re,os
from urllib import *
from time import sleep
import Queue
class spider:
  def __init__(self):
    self.use=[]
    self.queue=Queue.Queue

  def getlist(self,url):
    req=urllib2.Request(url)
    html=urllib2.urlopen(req).read()
    try:
        html=str(html).replace('\n','').decode('gbk')
        pattern=re.compile(r'<th class="subject hot">.*?<em>\[(.*?)\]</em>.*?href="(.*?)".*?>(.*?)</a>')
        items=pattern.findall(html)
        for ite in items:
            dict=[]
            dict.append(ite[0])
            dict.append(ite[1])
            dict.append(ite[2])
            self.use.append(dict);
    except Exception,e:
        print 'getList',str(e)
  def songImg(self,author,songUrl,fileName):
    fileName=str(fileName).replace(" ","")
    fileName=str(fileName).replace(':','')
    #os.getcwd()
    homeDir=r'E:\Bayes\GitResp\Gitar-Play\GitarPlay\Gitar'+"\\"+fileName+"-"+author
    try:
        ie=os.path.exists(homeDir)
        if not ie:
            os.makedirs(homeDir)
        else:
            print " Already Exits"
            return
    except Exception,e:
        print 'MakeDir Failed:'
    try:
        req=urllib2.Request(songUrl)
        html=urllib2.urlopen(req).read().decode('gbk')
        pattern=re.compile(r'<img src="(.*?)" alt=".*? /> ',re.I)
        items=pattern.findall(html)
        tag = 1
        for ite in items:
            imgurl=r"http://www.ccguitar.cn/"+ite
            #if tag == 1:print homeDir.decode('gbk')
            #homeDir=homeDir.replace(' ','')
            self.saveImg(imgurl,homeDir+"\\"+str(tag)+".gif")
            tag += 1
    except Exception,e:
        print 'songEmg',str(e)#,homeDir+"\\"+str(tag)+".gif"
  def saveImg(self,imageURL,fileName):
         u = urllib2.urlopen(imageURL)
         data = u.read()
         f = open(fileName, 'wb')
         f.write(data)
         f.close()
craw=spider()

for i in range(152,153):
    print "page: " + str(i)
    craw.getlist(r"http://www.ccguitar.cn/pu_list_0_"+str(i)+"_0_5_8.htm");
for i in range(len(craw.use)):
     lisTag=craw.use[i]
     print lisTag[1],lisTag[2]
     craw.songImg(lisTag[0].encode('gbk'),lisTag[1],lisTag[2].encode('gbk'))
