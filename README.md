# W.i.P Drawing-Generator  by inDialog <br/>
drawing generator for XY plotters. Made with Unity and developed to comnunicate over serial using  Grbl.

## Description<br/>
Build in unity it is composed of 3 main parts:  
* 1. script that creates a random pattern of points on a surface 
* 2. a script that moves a game object in-between the different points. This in tern has 4 cases that define the behaviour of the game object.  <br/>
		* a) Linear movements <br/>
		* b) arc moments <br/>
		* c) circular moments <br/>
		* d) Random Interpolation / Closes point <br/>
 * 4. a communication manager that establishes the link to the microcontroller and send position data to the arduino <br/><br/><br/>
# **Macros**
		Tab - add new points (creates error for the moment)<br/>
		K, V - screenshot and video <br/>
		R - Restart <br/>
	
<br/>
Check out the IMG folder for examples
https://github.com/In-dialog/W.i.P-Drawing-Generator/blob/master/img/1.jpg
<br/>
https://github.com/In-dialog/W.i.P-Drawing-Generator/blob/master/img/2.jpg
<br/>
https://github.com/In-dialog/W.i.P-Drawing-Generator/blob/master/img/3.jpg
<br/>
https://github.com/In-dialog/W.i.P-Drawing-Generator/blob/master/img/4.jpg
<br/>

# **To do** <br/>

	Sometimes The pen blocks in loop from time to time and dose not proceed to the next location
	
	When new points are created at once, the lists gets messy and it crashes
	
	Manage the circular movements of the XY-plotter with G02 commando for making smooth arch and circles
	
	Create new patters for the positioning of the points in space
	
	Add new movement options for the pen to interpolate between the points
	
	Create new patterns to positions points on a surface 
	
	Make a UI to manage :
		*The drawing size
		*Connection to Arduino
		*Speed of movement
		*Number of points
		*Types of drawings
<br/><br/>
## **If you want to know more about the projects** 
<br/><br/>
#poweredByID
www.in-dialog.com
<br/><br/>
contact@in-dialog.com
<br/><br/>
Patreon : https://www.patreon.com/indialogcollective
<br/><br/>
Instagram : https://www.instagram.com/indialogcollective/
<br/><br/>
Facebook : https://www.facebook.com/indialog.collectif
<br/><br/>
Thingiverse : https://www.thingiverse.com/InDialogCollective
<br/><br/>
Behance : https://www.behance.net/indialogcollective
<br/><br/>
