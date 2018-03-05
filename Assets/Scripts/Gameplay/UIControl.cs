using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class UIButton{
    public Image mainSprite;
    public Image outerCircle;

}
public class UIControl : MonoBehaviour {
    public UIButton boost, hBrake, forward, backward;

    public Color defaultOC_Color, defaultMain_Color, pressedOC_Color, pressedMain_Color;
    
	public void BoostPressed()
    {
        boost.mainSprite.color = pressedMain_Color;
        boost.outerCircle.color = pressedOC_Color;
    }

    public void BoostLeft()
    {
        boost.mainSprite.color = defaultMain_Color;
        boost.outerCircle.color = defaultOC_Color;
    }

    public void RevPressedc()
    {
        backward.mainSprite.color = pressedMain_Color;
        backward.outerCircle.color = pressedOC_Color;
    }

    public void RevLeft()
    {
        backward.mainSprite.color = defaultMain_Color;
        backward.outerCircle.color = defaultOC_Color;
    }

    public void AccelPressed()
    {
        forward.mainSprite.color = pressedMain_Color;
        forward.outerCircle.color = pressedOC_Color;
    }

    public void AccelLeft()
    {
        forward.mainSprite.color = defaultMain_Color;
        forward.outerCircle.color = defaultOC_Color;
    }

    public void hBrakePressed()
    {
        hBrake.mainSprite.color = pressedMain_Color;
        hBrake.outerCircle.color = pressedOC_Color;
    }

    public void hBrakeLeft()
    {
        hBrake.mainSprite.color = defaultMain_Color;
        hBrake.outerCircle.color = defaultOC_Color;
    }
}
