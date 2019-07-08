using System;
using UnityEngine;

public abstract class Timer
{
	protected int firstTimeCall = -1;
	public int prevTickCount;
	protected int currentTick;
	
	/*
	 * if timeOfActivity=-1 equals infinity
	 * */
	public int timeOfActivity;
	public int frequency;
	
	protected Timer ()
	{	
		this.firstTimeCall = TickCount;
	}

	public void SetNewLifePeriod(int lifePeriod) {
		this.frequency = 0;
		this.timeOfActivity = lifePeriod;
		this.Restart ();
	}

	public bool CheckEnoughTimeLeft () {
		currentTick = TickCount;	
		int timeOffset = currentTick - firstTimeCall;
		if (timeOfActivity > 0 && timeOffset > timeOfActivity) {
			return false;	
		}
		
		bool enoughTimeLeft = false;
		
		if (GetFrequencyOffset () > frequency) {	
			enoughTimeLeft = true;
		}
		
		return enoughTimeLeft;
	}

    public int GetFrequencyOffset ()
    {
        return currentTick - prevTickCount;
    }
	
	public bool EnoughTimeLeft() {
		bool enoughTimeLeft = CheckEnoughTimeLeft ();
		if (enoughTimeLeft) {
			prevTickCount = currentTick;
		}

		return enoughTimeLeft;
	}
	
	public float GetFrequencyProgress () {
		currentTick = TickCount;	
		float offset = currentTick - prevTickCount;
		return offset * 100 / frequency;
	}
	
	public void Restart() {
		this.firstTimeCall = TickCount;
		prevTickCount = this.firstTimeCall;
	}
	
	public bool IsActive {
		get {
			if (timeOfActivity < 0) {
				return true;
			}
			currentTick = TickCount;
			int timeOffset = currentTick - firstTimeCall;			
			return timeOffset < timeOfActivity;
		}
	}
	
	public void Deactivate () {
		firstTimeCall = firstTimeCall - timeOfActivity;
	}
	
	public float GetProgressPercent () {
		if (!IsActive) 
		{
			return 0;
		}
		
		currentTick = TickCount;
		int timeOffset = currentTick - firstTimeCall;
		
		return timeOffset * 100 / timeOfActivity;
	}
	
	//public void YieldTime ()
	//{
	//	prevTickCount += 2 * frequency + Randomizer.Next(frequency);
	//}

	public void ForceTime ()
	{
		prevTickCount -= frequency;
	}

	public int TimeOfActivity {
		get {
			return timeOfActivity;
		}
	}

	public int MiliSecondsOfLife {
		get {
			currentTick = TickCount;
			int timeOffset = currentTick - firstTimeCall;	
			return timeOfActivity - timeOffset;
		}
	}

	protected abstract int TickCount {
		get;
	}
}

public class SystemTimer : Timer
{
	public static SystemTimer Create(int frequency, int timeOfActivity) {
		SystemTimer t = new SystemTimer();
		t.frequency = frequency;
		t.timeOfActivity = timeOfActivity;
		return t;
	}
	
	public static SystemTimer CreateFrequency(int frequensy) {
		return Create(frequensy, -1);
	}

	public static SystemTimer CreateLifePeriod(int lifePeriod, bool isActive = true) {
		SystemTimer t = Create(0, lifePeriod);
		if (!isActive) {
			t.Deactivate ();
		}
		return t;
	}
	
	public static SystemTimer CreateEmptyLifePeriod () {
		return Create(0, 100);
	}

	protected override int TickCount {
		get {
			return System.Environment.TickCount;
		}
	}
}

public class TyranTimer : Timer {

	public static TyranTimer CreateFrequencyWithLife(int frequency, int timeOfActivity) {
		TyranTimer t = new TyranTimer();
		t.frequency = frequency;
		t.timeOfActivity = timeOfActivity;
		return t;
	}
	
	//public static TyranTimer CreateRandomizeFrequency (int frequency, int random) {
	//	int finalFrequency = frequency;
	//	int offset = Randomizer.Next (random * frequency / 100 * 2) - random * frequency / 100;
	//	TyranTimer result = CreateFrequencyWithLife(finalFrequency, -1);
	//	result.firstTimeCall += offset;
	//	return result;
	//}
	
	public static TyranTimer CreateFrequency(int frequency) {
		return CreateFrequencyWithLife(frequency, -1);
	}
	
	public static TyranTimer CreateFrequency(float frequency) {
		return CreateFrequencyWithLife((int)(frequency * 1000), -1);
	}
	
	public static TyranTimer CreateLifePeriod(int lifePeriod, bool isActive = true) {
		TyranTimer t = CreateFrequencyWithLife(0, lifePeriod);
		if (!isActive) {
			t.Deactivate ();
		}
		return t;
	}
	
	public static TyranTimer CreateEmpty () {
		return CreateFrequencyWithLife(0, 100);
	}

	protected override int TickCount {
		get {
			return CurrentTime;
		}
	}

	private static int time = 10;
	private static bool active = true;
	public static float deltaTime;

    private static float _timeScale = 1f;
    public static float TimeScale
    {
        set
        {
            _timeScale = value;
        }
        get
        {
            return _timeScale;
        }
    }

    public static void Reset()
    {
        time = 0;
    }

    public static void OnUpdate () {
		if (!active) {
			return;
		}

		deltaTime = Time.deltaTime;
		float timeAccum = 1000 * deltaTime;
		time += (int) timeAccum;

        TimeScale = Time.timeScale;
	}	
	
	public static bool Active {
		get {
			return active;
		}
		set {
			active = value;
		}
	}
	
	public static int CurrentTime {
		get {
			return time;
		}
		set {
			time = value;
		}
	}
	
	public static void OnNewSceneStarted ()
	{
//		time = 0;
		Active = true;
	}
}


