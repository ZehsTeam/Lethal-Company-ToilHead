namespace com.github.zehsteam.ToilHead;

public class ToilHeadData
{
    public string planetName;
    public ToilHeadConfigData configData;

    public ToilHeadData(string planetName, ToilHeadConfigData configData)
    {
        this.planetName = planetName;
        this.configData = configData;
    }
}
