# coding:utf-8
# __author__ = 'dandong'
import urllib2, re, os
import Queue


class Spider:
    def __init__(self):
        self.target = []
        self.queue = Queue.Queue

    def getlist(self, url):
        req = urllib2.Request(url)
        html = urllib2.urlopen(req).read()
        try:
            html = str(html).replace('\n', '').decode('gbk')
            pattern = re.compile(r'<th class="subject hot">.*?<em>\[(.*?)\]</em>.*?href="(.*?)".*?>(.*?)</a>')
            items = pattern.findall(html)

            for ite in items:
                son_list = [ite[0], ite[1], ite[2]]
                self.target.append(son_list)

        except Exception, e:
            print 'ERROR,getList faile.[info={0}]'.format(str(e))

    def craw_song_img(self, author, songUrl, fileName):
        fileName = str(fileName).replace(" ", "").replace(':', '')
        homeDir = r'E:\GitarPic' + "\\" + fileName + "-" + author
        try:
            ie = os.path.exists(homeDir)
            if not ie:
                os.makedirs(homeDir)
            else:
                # print " Already Exits"
                return
        except Exception, e:
            print 'MakeDir Failed:[info={0}]'.format(str(e))

        try:
            req = urllib2.Request(songUrl)
            html = urllib2.urlopen(req).read().decode('gbk')
            pattern = re.compile(r'<img src="(.*?)" alt=".*? /> ', re.I)
            items = pattern.findall(html)
            tag = 1
            if len(items) == 0:
                print "Its error"
            for ite in items:
                imgurl = r"http://www.ccguitar.cn/" + ite
                # if tag == 1:print homeDir.decode('gbk')
                # homeDir=homeDir.replace(' ','')
                self.save_img(imgurl, homeDir + "\\" + str(tag) + ".gif")
                tag += 1
        except Exception, e:
            print 'songEmg', str(e)  # ,homeDir+"\\"+str(tag)+".gif"

    def save_img(self, imageURL, fileName):
        u = urllib2.urlopen(imageURL)
        data = u.read()
        f = open(fileName, 'wb')
        f.write(data)
        f.close()

if __name__ == "__main__":
    print "ok stat craw pictures!"

    craw = Spider()
    for i in range(10, 12):
        print "Star process page: " + str(i)
        craw.getlist(r"http://www.ccguitar.cn/pu_list_0_" + str(i) + "_0_5_8.htm")

    for i in range(len(craw.target)):
        lisTag = craw.target[i]
        print lisTag[1], lisTag[2]
        craw.craw_song_img(lisTag[0].encode('gbk'), lisTag[1], lisTag[2].encode('gbk'))