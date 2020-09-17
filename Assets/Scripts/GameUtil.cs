using System;

public static class GameUtil {
    public static string ZeroOnLeft(int value) {
        return value < 10 ? "0" + value.ToString() : value.ToString();
    }

    public static string ConvertScore(float timer) {
        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        int milliseconds = (int)(Math.Floor((timer - (seconds + minutes * 60)) * 100));
        return ZeroOnLeft(minutes) +":"+ ZeroOnLeft(seconds) + ":" + ZeroOnLeft(milliseconds);
    }

}
