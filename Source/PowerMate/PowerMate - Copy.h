//======================================================================================================
//======================================================================================================
//======================================================================================================
//======================================================================================================
//======================================================================================================

//Trace.WriteLine(slider.Value.ToString());

#define _CRT_SECURE_NO_WARNINGS

#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

 
#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Diagnostics;
using namespace System::Threading;

#include "EventsHelper.h"

namespace HidDevice
{
	
	public ref class PowerMate
	{
		
//=======Enums ===========================================================================================

       #pragma region Enums

		

		private: enum class HidAttributes			// Griffin PowerMate Knob ID
		{
			VendorID		= 0x077d,
			ProductID		= 0x0410,
			VersionNumber	= 0x0311
		};

		public: enum class RotationalDirection
		{
			Left,
			Right
		};

		public: enum class ButtonState
		{
			Up,
			Down
		};
		#pragma endregion

//======Data members============================================================================================

		#pragma region Data members

 
		public: 
			delegate void RotationHandler(RotationalDirection direction, int value, int value1, int value2, int value3);  // SEND TO FORM:  direction, currentrotationvalue, txsend
			delegate void ButtonHandler(ButtonState bs, int value, int value1, int value2);                               // SEND TO FORM: bs, bandsel

			delegate void SignalHandler(int value, int value1);  // send to form Signal strength : corrected for display, actual -xxxdBm


			// ^ indicates a handle to a routine.
		private:
			SignalHandler^ signalDelegate;
			RotationHandler^ rotationDelegate;				// direction, rot value, txsend, value2, value3
			ButtonHandler^ buttonDelegate;					// button state, bandsel, value1, value2
            HANDLE handleToDevice;							// device handle of the PowerMate Knob
		    Thread^ inputThread;							// inputthread routine 
		    ManualResetEvent^ exitEvent;					//

			HANDLE hPort;									// COM port handle


 //-----------------------------------------------------


			int rotvalue;
			int rotationValue;								// is value from rotationHandler
			int lastrotation;
			int xxx;										// used to pass com port value back to this program
			int KBON;										// 1=detect knob, 0=dont

		

            HWND hNWnd;										// Window Handle
 			HWND hTWnd;									    // child window handle
            DWORD threadID;									// Widnow Thread	

		
			int TIMER_CAT;

	


		#pragma endregion



 //============================================================================================================
		#pragma region Constructor

		public: PowerMate() : handleToDevice(NULL),inputThread(nullptr),exitEvent(nullptr),signalDelegate(nullptr),rotationDelegate(nullptr),buttonDelegate(nullptr), rotationValue(0)
		{

		}

		#pragma endregion
	
//======Initialize============================================================================================

		#pragma region Initialize

		public: bool Initialize()
		{
			

	
        //==============PowerMate====================================================
		
         array<Process^>^ powerMateProcess = Process::GetProcessesByName("PowerMate");

			if(powerMateProcess->Length > 0)
			{
				HANDLE hp = OpenProcess(SYNCHRONIZE | PROCESS_TERMINATE,FALSE,powerMateProcess[0]->Id);
				
		
				HWND hWnd = FindWindow(NULL, L"PowerMateWnd");
				
				Trace::WriteLine("DETECT IF PowerMate RUNNING");

				if(NULL != hWnd)
				{
					Trace::WriteLine("PowerMate is RUNNING");
					
					PostMessage(hWnd, WM_CLOSE,0,0);

					if( WaitForSingleObject(hp, 5000)!= WAIT_OBJECT_0)
					{
						Trace::WriteLine("Failure to close PowerMate.exe");
						return false;
					}
					else
						Trace::WriteLine("PowerMate.exe has been closed");
				}
			}
	


		//=======================================================================
		//=======================================================================
		//=======================================================================
		//=======================================================================
		// INITIALIZE POWERMATE KNOB


			GUID hidClass;
			HidD_GetHidGuid(&hidClass);									// returns the device interface GUID for HIDClass devices.
			
																		// retrieves a device information set that contains all the
																		// devices of a specified class (hint: our HIDClass)
			HDEVINFO hDevInfoSet = SetupDiGetClassDevs(&hidClass,
													   NULL,					// retrieve dev info for all instances
													   NULL,				    // hWnd parent
													   DIGCF_PRESENT |			// only present devices
													   DIGCF_INTERFACEDEVICE);	// that expose hid interface

			
			
			if (INVALID_HANDLE_VALUE == hDevInfoSet)
			{
				Trace::WriteLine("SetupDiGetClassDevs");
				return false;
			}

			
			SP_INTERFACE_DEVICE_DATA interfaceData;												 // enumerate devices looking for hid interfaces
			interfaceData.cbSize = sizeof(interfaceData);

																							// poll device manager until no matching devices left
			for(int i = 0; SetupDiEnumDeviceInterfaces(hDevInfoSet,NULL,&hidClass,i,&interfaceData); ++i)
			{
					
					DWORD bufferLength;                                                     // retrieve buffer size for interface detail data

					                                                                         // retrieves detailed information about a specified device interface
					SetupDiGetDeviceInterfaceDetail(hDevInfoSet,&interfaceData,NULL,0,&bufferLength,NULL);

					PSP_INTERFACE_DEVICE_DETAIL_DATA interfaceDetail = (PSP_INTERFACE_DEVICE_DETAIL_DATA)new char[bufferLength];

					interfaceDetail->cbSize = sizeof(SP_INTERFACE_DEVICE_DETAIL_DATA);
					
					

					                                                                              // now get the interface detail data
					if( !SetupDiGetDeviceInterfaceDetail(hDevInfoSet,&interfaceData,interfaceDetail,bufferLength,NULL,NULL))
					{
						delete interfaceDetail;
						SetupDiDestroyDeviceInfoList(hDevInfoSet);
						break;
					}

         //==================================================================================================
					// - now that we have the device path, open device
					// - use FILE_FLAG_OVERLAPPED for simultaneous read/write
					// - the system does not maintain the file pointer,
					//   therefore you must pass the file position to the
					//   read and write functions in the OVERLAPPED structure
					
					HANDLE hDevice = CreateFile(interfaceDetail->DevicePath,GENERIC_READ | GENERIC_WRITE,FILE_SHARE_READ | FILE_SHARE_WRITE,
							                        NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED ,NULL);

					if( INVALID_HANDLE_VALUE == hDevice )
					{
						delete interfaceDetail;
						continue; // keep searching
					}
					else
					{
						HIDD_ATTRIBUTES hidAttr;
						BOOLEAN result = HidD_GetAttributes(hDevice,&hidAttr);

						if (result)
						{
									// this is what we are looking for (PowerMate Knob)
							        //	VendorID		= 0x077d,
									// ProductID		= 0x0410,
									// VersionNumber	= 0x0311
									// grab the first matching device we find and return

							if( (int)HidAttributes::ProductID == hidAttr.ProductID && (int)HidAttributes::VendorID  == hidAttr.VendorID )
							{
								Trace::WriteLine("FOUND GRIFFIN KNOB");
								handleToDevice = hDevice;

								KBON = 1;

								delete interfaceDetail;
								SetupDiDestroyDeviceInfoList(hDevInfoSet);

								exitEvent = gcnew ManualResetEvent(false);


								inputThread = gcnew Thread( gcnew ThreadStart(this, &PowerMate::InputThread));		// THREAD for InputThread
								inputThread->Priority = ThreadPriority::Normal;
								inputThread->Start();

								return true;
							} // found knob

						} // result

					} // valid handle

				

			} // FOR LOOP

			KBON = 0;	// no knob found

			//delete interfaceDetail;
			SetupDiDestroyDeviceInfoList(hDevInfoSet);

			exitEvent = gcnew ManualResetEvent(false);

//			inputThread = gcnew Thread( gcnew ThreadStart(this, &PowerMate::InputThread));		// THREAD for InputThread
//			inputThread->Priority = ThreadPriority::Normal;
	//		inputThread->Start();
			return false;


		} // end Initialize


		#pragma endregion

//=====Shutdown =================================================================================================

		#pragma region Shutdown
		public: void Shutdown()
		{
			
			
				Trace::WriteLine("ShutDown-1");

			if( nullptr != exitEvent)
			{
				Trace::WriteLine("ShutDown-2");
				exitEvent->Set();
				inputThread->Join();
                 Trace::WriteLine("ShutDown-3");

			}

			Trace::WriteLine("ShutDown-4");
			if( NULL != handleToDevice )	CloseHandle(handleToDevice);     // close PowerMate	

			Trace::WriteLine("ShutDown-5");
		} // shutdown


		#pragma endregion


//=================================================================================================================
// Pulse speed slider value
//=================================================================================================================
		#pragma region PulseSpeed
		// - use slider with range 0-24
		// TODO: refactor and pass enum param, for example:
		//		 Pulse::TwoPerSecond or Pulse::OnceEvery28Seconds
		public: property Byte PulseSpeed 
		{
			void set(Byte value)
			{
				if( NULL != handleToDevice )
				{
					UCHAR reportBuffer[9] = { 0 };

					reportBuffer[0] = 0x00;     // 
					reportBuffer[1] = 0x41;		//  LED Brightness
					reportBuffer[2] = 0x01;		// 
					reportBuffer[3] = 0x04;		// command type
					reportBuffer[4] = 0x00;		// select table

					// set pulse rate
					// byte 5; 0 = divide, 1 = normal speed, 2 = multiply
					// byte 6; value

					int pos = value;  // pulse value sent from FORM slider (not there anymore)

					if (pos < 8)
					{
						reportBuffer[5] = 0;
						reportBuffer[6] = (7-pos) * 2;
					}
					else if (pos > 8)
					{
						reportBuffer[5] = 2;
						reportBuffer[6] = (pos-8) * 2;
					}
					else				
					{
						reportBuffer[5] = 1;				// if slider = 8 then do this
						reportBuffer[6] = 0;
					}

					HidD_SetFeature(handleToDevice, reportBuffer, sizeof(reportBuffer));		// send new pulse data back to PowerMate Knob
				}
			}
		}
		#pragma endregion

//====LED Brightness==================================================================================================

		#pragma region LedBrightness
		public: property Byte LedBrightness
		{
			
			void set(Byte value)   // when trying to set the LED value
			{
				if( NULL != handleToDevice )
				{
					HANDLE hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);

					OVERLAPPED overLap;							// async I/O sturcture
					ZeroMemory(&overLap, sizeof(overLap));		// 0 overlap
					overLap.hEvent = hEvent; 

					UCHAR reportBuffer[2];
					reportBuffer[0] = 0;		//
					reportBuffer[1] = value;    // LED brightness

					

					DWORD dwBytesTransmitted = 0;
					BOOL result = WriteFile(handleToDevice,reportBuffer,sizeof(reportBuffer),&dwBytesTransmitted,&overLap);  // write data to Knob

					DWORD dw = WaitForSingleObject(hEvent, 40); // wait  40 mSec for signal
								
					switch(dw)
					{
						case WAIT_OBJECT_0:
							break;

						case WAIT_TIMEOUT:
							break;
					}

					CloseHandle(hEvent);
					
				}
			}

			Byte get()   // when trying to read the LED value
			{ 
				UCHAR reportBuffer[7] = { 0 };

				if( NULL != handleToDevice )
				{
					BOOLEAN error = HidD_GetInputReport(handleToDevice, reportBuffer, sizeof(reportBuffer));  // read KNOB

					if( FALSE == error )
						Trace::WriteLine("failure to get input report - brightness");	
				}

				return reportBuffer[4];
			}
		}
		#pragma endregion





//==================================================================================================================
// Pulse ON/OFF
//==================================================================================================================

	#pragma region PulseOn
		
        public: property bool PulseOn
		{
			void set(bool value)
			{
				if( NULL != handleToDevice )
				{
					UCHAR reportBuffer[9] = { 0 };
					reportBuffer[0] = 0x00;
					reportBuffer[1] = 0x41;     // LED brightness
					reportBuffer[2] = 0x01;
					reportBuffer[3] = 0x03;	    // command type
					reportBuffer[4] = 0x00;

					if( true == value )
						reportBuffer[5] = 0x01;	  // command value
					else
						reportBuffer[5] = 0x00;	  // command value

					HidD_SetFeature(handleToDevice, reportBuffer, sizeof(reportBuffer)); //
				}
			} // set


		}
		#pragma endregion

//====Events==================================================================================================

		#pragma region Events

		public: event RotationHandler^ RotateEvent
		{
			void add(RotationHandler^ value)
			{
				rotationDelegate += value;

	
			}

			void remove(RotationHandler^ value)
			{
				rotationDelegate -= value;
			}
		}



		public: event ButtonHandler^ ButtonEvent
		{
			void add(ButtonHandler^ value)
			{
				buttonDelegate += value;
			}

			void remove(ButtonHandler^ value)
			{
				buttonDelegate -= value;
			}
		}
				

		public: event SignalHandler^ SignalEvent
		{
			void add(SignalHandler^ value)
			{
				signalDelegate += value;
			}

			void remove(SignalHandler^ value)
			{
				signalDelegate -= value;
			}
		}
				
		#pragma endregion


				
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
// InputThread  is a running thread
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================
//==================================================================================================================


		#pragma region InputThread

		private: void InputThread()
		{
			
			char reportBuffer[8] = {0};
				
		
			DWORD dwBytesRead = 0;									//
			BOOL result = FALSE;									// ???

			HANDLE hEvent = CreateEvent(NULL,FALSE,FALSE,NULL);		// ???
			
			OVERLAPPED overLap;										// ???
			ZeroMemory( &overLap, sizeof(overLap));					// ???
			overLap.hEvent = hEvent;								// ???
			


			while(true)
			{
				
				if (KBON == 1)	result = ReadFile(handleToDevice, reportBuffer, sizeof(reportBuffer), &dwBytesRead, &overLap);// async call
		
				DWORD dw = WaitForSingleObject(hEvent, TIMER_CAT);  // wait for 200=200mSec (was 2 seconds)
				
	
				switch(dw)
				{
					case WAIT_OBJECT_0:
					{
						
						if ((reportBuffer[1] == 1) )   // test for pushbutton
						{
					
							ButtonState bs = ButtonState::Down;
							EventsHelper::Tire(buttonDelegate, bs, 0, 0, 0);  // send button state, and bandsel to form
							Trace::WriteLine("DETECTED DWN");
					
						}
						else if ( reportBuffer[2] == 0 && reportBuffer[1] == 0)  // test for knob UP
						{
							ButtonState bs = ButtonState::Up;
							EventsHelper::Tire(buttonDelegate, bs, 0, 0, 0);

							Trace::WriteLine("DETECTED UP");
		
						}
		
						rotvalue = reportBuffer[2];

						if ((reportBuffer[2] != 0)  )                    // test for knob rotation
						{
						
							Trace::WriteLine("Reportbuffer " + reportBuffer[2]);
                     	
							EventsHelper::Tire(rotationDelegate,(reportBuffer[2] < 0) ? RotationalDirection::Left : RotationalDirection::Right, 0, rotvalue, 0,0); // send direction, rotationvalue, txsend
							reportBuffer[2] = 0;

						} // knob rotation


					} break;  // case wait_object 0


		//==================================================================
					case WAIT_TIMEOUT:
					{
			
						if( exitEvent->WaitOne(20,false))  // wait for event to finish
						{
							CloseHandle(hEvent);
							Trace::WriteLine("hEvent closed");
							return;
						}

					} break;

				} // end switch

			} // end while

		} // end method

#pragma endregion

//======================================================================================================

	}; // end class PowerMate

} // end namespace