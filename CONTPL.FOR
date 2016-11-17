      include 'fgraph.fi'
      subroutine contpl(nps,labl,nchlab,on,dlen)
      include 'fgraph.fd'
c***********************************************************************
c          draw and label contour lines 
c 
c          (x,y)    = arrays of (x,y) points of line 
c          nps      = # of points in (x,y)
c          labl     = array containing label characters (2 per word)
c          nchlab   = # chars in label
c          on       = parameter to dashit describing line style
c          dlen     = parameter to dashit
c***********************************************************************
// common block called xyval   is used to find the x[] and y[] values passed between subroutines


        common /xyval/ x[900],y[900]

        character labl*[*]




        call charget(chxx,chyy)

        call charworl(chx,chy)       !  get character size in world units

        size = chx * 1.1

        nchar = nchlab

        call rblankc(labl,nchar)

c          locate label at center of line

        len = getgtextextent( labl[1:nchar] )

        sizelab = (float)len * chx / chxx + size           // convert len from int to float


c          find line length

        xlen = 0.

//=================================================================

      for (int i = 2; i <= nps; i++)
      {

         xlen = xlen + Math.sqrt( Math.Pow((x[i] - x[i-1]),2) + Math.Pow((y[i] - y[i-1]),2)  )
      }

//=================================================================



c******************************************************************
c          eliminate short lines (short=width of 1 character)
      if(xlen < size) 
      {
        return;
      }

c******************************************************************
        hlen = xlen / 2.

        if( sizelab < (hlen / 2.0) ) goto PLOT40

PLOT15: call dashit(x, y, nps ,on ,dlen)

        return;
        

PLOT40: hlen = hlen - sizelab / 2.

        xlen = 0.

//=================================================================

      for (int i = 2; i <= nps; i++)
      {
       
        dl = Math.sqrt( Math.Pow((x[i] - x[i-1]),2) + Math.Pow((y[i] - y[i-1]),2) )

        xlen = xlen + dl

        if(xlen > hlen) break;


      }
//=================================================================

        xlen = xlen - dl

        frac = (hlen - xlen) / dl

        xc = x[i-1] + (x[i] - x[i-1]) * frac
        yc = y[i-1] + (y[i] - y[i-1]) * frac

        ndex = i

        xsave = x[i]
        ysave = y[i]

        x[i] = xc
        y[i] = yc

	xc1 = xc
	yc1 = yc

        call dashit(x,y,ndex,on,dlen)

        x[i] = xsave
        y[i] = ysave



//=================================================================
c          find (xp,yp) such that dist (xc,yc)-(xp,yp) = sizelab


      for (int i = ndex; i <= nps; i++)
      {
          dl = Math.sqrt( Math.Pow((x[i] - xc),2) + Math.Pow((y[i] - yc),2) )

          if(dl > sizelab) goto 60

      }
 //=================================================================

        goto 15

c          (xp,yp) is between (x(nn),y(nn)) & (x(nn+1),y(nn+1))
 60     nn = i

        x1 = x[nn - 1]
        y1 = y[nn - 1]

        x2 = x[nn]
        y2 = y[nn]

//=================================================================


      for (int i = 1; i <= 15; i++)
      {
       
        xp = (x1 + x2) / 2.
        yp = (y1 + y2) / 2.

        d = Math.sqrt( Math.Pow((xp - xc),2) + Math.Pow((yp - yc),2) )

        if(d < sizelab) goto 65

        x2 = xp
        y2 = yp

        goto 70

 65     x1 = xp
        y1 = yp

      }

//=================================================================

        dx = xp - xc
        dy = yp - yc

        ang = Math.Atan2(dy,dx) / .0174533       // takes real for floating values y,x

        fact = (nchar + 1) * 2

        dx = dx / fact
        dy = dy / fact

        if((ang > 90.) || (ang < -90.)) goto 80

        a = (ang - 90.) * .0174533

        xc = xc + dx + Math.Cos(a) * size / 2.
        yc = yc + dy + Math.Sin(a) * size / 2.

        goto 90

 80:    ang = ang + 180.

        a = (ang - 90.) * .0174533

        xc = xp - dx + Math.Cos(a) * size / 2.
        yc = yp - dy + Math.Sin(a) * size / 2.



c       call symbol(xc,yc,chx,chy,labl,ang,nchar)

90:     x[nn-1] = xp
        y[nn-1] = yp

c           draw the rest of the line
C           draw dashed lines thru array (x,y)
C           n =
c          on = fraction of interval pen down (0 < on <= 1.)
c          dlen = interval length
        call dashit( x[nn - 1],y[nn - 1], (nps - nn + 2), on, dlen )

	xc = (xc1 + xp) / 2.
	yc = (yc1 + yp) / 2.


        //  convert to screenunits: input = xc,yc  output = u, v
	call plotxy(xc, yc, u, v)


//======================================================================
C        Plot to (IX,IY) with pen up(IPEN=3), or pen down(IPEN=2)
C         (IX,IY) in screen coordinates
 	call plotabs( (u - float(len) / 2.) , (v + chyy / 2.), 3)    //  this is a PEN UP condition

	call textc(nchar,labl)  // print text

        return;

