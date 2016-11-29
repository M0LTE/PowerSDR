      include 'fgraph.fi'
      subroutine contpl(nps,labl,nchlab,on,dlen)
      include 'fgraph.fd'

c***********************************************************************
      common /xyval/ x(900),y(900)
      character labl*(*)
      call charget(chxx,chyy) 
      call charworl(chx,chy)       !  get character size in world units
      size=chx*1.1 
        nchar=nchlab  
        call rblankc(labl,nchar)
c          locate label at center of line
      len=getgtextextent(labl(1:nchar))
        sizelab=float(len)*chx/chxx + size
c          find line length 
        xlen=0. 

        do 10 i=2,nps 

 10     xlen=xlen + sqrt((x(i)-x(i-1))**2 + (y(i)-y(i-1))**2) 

c******************************************************************
c          eliminate short lines (short=width of 1 character)
    
	  if(xlen.lt.size) go to 150 
c******************************************************************
        hlen=xlen/2.
		 
        if(sizelab.lt.hlen/2.0) go to 40  

 15     call dashit(x,y,nps,on,dlen)  

        go to 150 

 40     hlen=hlen-sizelab/2. 
      
        xlen=0. 

        do 20 i=2,nps 
          dl=sqrt((x(i)-x(i-1))**2 + (y(i)-y(i-1))**2)
          xlen=xlen+dl
          if(xlen.gt.hlen) go to 30     
 20    continue

 30     xlen=xlen-dl
        frac=(hlen-xlen)/dl 

        xc=x(i-1) + (x(i)-x(i-1))*frac
        yc=y(i-1) + (y(i)-y(i-1))*frac

        ndex=i

        xsave=x(i)
        ysave=y(i)

        x(i)=xc
        y(i)=yc

	    xc1=xc
	    yc1=yc

        call dashit(x,y,ndex,on,dlen)   

        x(i)=xsave
        y(i)=ysave

c          find (xp,yp) such that dist (xc,yc)-(xp,yp) = sizelab

        do 50 i=ndex,nps
        dl=sqrt((x(i)-xc)**2 + (y(i)-yc)**2)
        if(dl.gt.sizelab) go to 60      
 50     continue
        go to 15

c          (xp,yp) is between (x(nn),y(nn)) & (x(nn+1),y(nn+1)) 
 60     nn=i
        x1=x(nn-1)
        y1=y(nn-1)
        x2=x(nn)
        y2=y(nn)
        do 70 i=1,15       
        xp=(x1+x2)/2. 
        yp=(y1+y2)/2. 
        d=sqrt((xp-xc)**2 + (yp-yc)**2) 
        if(d.lt.sizelab) go to 65 
        x2=xp 
        y2=yp 
        go to 70
 65     x1=xp 
        y1=yp 
 70     continue
        dx=xp-xc
        dy=yp-yc
        ang=atan2(dy,dx)/.0174533 
        fact=(nchar+1)*2
        dx=dx/fact
        dy=dy/fact
        if(ang.gt.90. .or. ang.lt.-90.) go to 80
        a=(ang-90.)*.0174533
        xc=xc+dx+cos(a)*size/2. 
        yc=yc+dy+sin(a)*size/2. 
        go to 90
 80     ang=ang+180.
        a=(ang-90.)*.0174533
        xc=xp-dx + cos(a)*size/2. 
        yc=yp-dy + sin(a)*size/2. 
90      continue
cc      call symbol(xc,yc,chx,chy,labl,ang,nchar)
        x(nn-1)=xp
        y(nn-1)=yp
c           draw the rest of the line
        call dashit(x(nn-1),y(nn-1),nps-nn+2,on,dlen)
	xc=(xc1+xp)/2.
	yc=(yc1+yp)/2.
	call plotxy(xc,yc,u,v)
	call plotabs(u-float(len)/2.,v+chyy/2.,3)
	call textc(nchar,labl)
 150    return
        end 
