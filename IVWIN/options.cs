using System;

public enum DrawMode {
    DEFALT,
    ORIGINAL,
    WIDTH_MATCH,
    HEIGHT_MATCH,
    FRAME_MATCH
}

public class LoadOption
{

	public LoadOption()
	{
	}

    public boolean mangaModeFLag = false;
    public DrawMode drawMode = DrawMode.DEFALT;



}
