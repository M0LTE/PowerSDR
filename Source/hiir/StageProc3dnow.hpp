/*****************************************************************************

        StageProc3dnow.hpp
        Copyright (c) 2005 Laurent de Soras

--- Legal stuff ---

This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.

*Tab=3***********************************************************************/



#if defined (hiir_StageProc3dnow_CURRENT_CODEHEADER)
	#error Recursive inclusion of StageProc3dnow code header.
#endif
#define	hiir_StageProc3dnow_CURRENT_CODEHEADER

#if ! defined (hiir_StageProc3dnow_CODEHEADER_INCLUDED)
#define	hiir_StageProc3dnow_CODEHEADER_INCLUDED



/*\\\ INCLUDE FILES \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/

#include	"hiir/StageData3dnow.h"

#if defined (_MSC_VER)
	#pragma inline_depth (255)
#endif



namespace hiir
{



/*\\\ PUBLIC \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



#if defined (_MSC_VER)
#pragma warning (push)
#pragma warning (4 : 4799)
#endif



template <int CUR>
void	StageProc3dnow <CUR>::process_sample_pos ()
{
	StageProc3dnow <CUR - 1>::process_sample_pos ();

	enum {	PREV_CELL	= (CUR - 1) * sizeof (StageData3dnow)	};
	enum {	CURR_CELL	=  CUR      * sizeof (StageData3dnow)	};

	__asm
	{
		movq				mm1, [edx + PREV_CELL + 1*8]
		movq				[edx + PREV_CELL + 1*8], mm0

		pfsub				mm0, [edx + CURR_CELL + 1*8]
		pfmul				mm0, [edx + CURR_CELL + 0*8]
		pfadd				mm0, mm1
	}
}

template <>
hiir_FORCEINLINE void	StageProc3dnow <0>::process_sample_pos ()
{
	// Nothing, stops the recursion
}



template <int CUR>
void	StageProc3dnow <CUR>::process_sample_neg ()
{
	StageProc3dnow <CUR - 1>::process_sample_neg ();

	enum {	PREV_CELL	= (CUR - 1) * sizeof (StageData3dnow)	};
	enum {	CURR_CELL	=  CUR      * sizeof (StageData3dnow)	};

	__asm
	{
		movq				mm1, [edx + PREV_CELL + 1*8]
		movq				[edx + PREV_CELL + 1*8], mm0

		pfadd				mm0, [edx + CURR_CELL + 1*8]
		pfmul				mm0, [edx + CURR_CELL + 0*8]
		pfsub				mm0, mm1
	}
}

template <>
hiir_FORCEINLINE void	StageProc3dnow <0>::process_sample_neg ()
{
	// Nothing, stops the recursion
}



#if defined (_MSC_VER)
#pragma warning (pop)
#endif



/*\\\ PROTECTED \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



/*\\\ PRIVATE \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/



}	// namespace hiir



#endif	// hiir_StageProc3dnow_CODEHEADER_INCLUDED

#undef hiir_StageProc3dnow_CURRENT_CODEHEADER



/*\\\ EOF \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*/
