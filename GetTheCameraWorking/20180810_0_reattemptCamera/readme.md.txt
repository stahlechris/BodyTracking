
	[DllImport("DllHelloWorld")]
	public static extern int DisplayHelloFromDLL();
	Debug.Log(DisplayHelloFromDLL());

Dll simple returns an int 987
Need to print it to see it working
Was just calling DisplayHelloFromDLL() by itself
That's why no errors and no success
