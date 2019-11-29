# ** W.i.P Drawing-Generator **  by @in-dialog
drawing generator for XY plotters. Made with Unity and developed to comnunicate over serial using  Grbl.
# Description

Generates line drawing that can  be used to drive any machine using grbl protocol over serial.   <br/>
Build in unity it is composed of 3 main parts:  <br/><br/>
	1. script that creates a random pattern of points on a surface <br/>
	2. a script that moves a game object in-between the different points. This in tern has 4 cases that define the behaviour of the game object.  <br/>
		a) Linear movements <br/>
		b) arc moments <br/>
		c) circular moments <br/>
		d) Random Interpolation / Closes point <br/>
  	4. a communication manager that establishes the link to the microcontroller and send position data to the arduino <br/><br/>
	Macros <br/>
	Tab - add new points (creates error for the moment)<br/>
	K, V - screenshot and video <br/>
	R - Restart <br/>
	
<br/>
	Check out the IMG folder for examples
<br/> <br/> 
