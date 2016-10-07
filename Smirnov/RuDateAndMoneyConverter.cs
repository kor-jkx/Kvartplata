// Decompiled with JetBrains decompiler
// Type: Kvartplata.Smirnov.RuDateAndMoneyConverter
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;

namespace Kvartplata.Smirnov
{
  public static class RuDateAndMoneyConverter
  {
    private static string[] monthNamesGenitive = new string[13]{ "", "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };
    private static string zero = "ноль";
    private static string firstMale = "один";
    private static string firstFemale = "одна";
    private static string firstFemaleAccusative = "одну";
    private static string firstMaleGenetive = "одно";
    private static string secondMale = "два";
    private static string secondFemale = "две";
    private static string secondMaleGenetive = "двух";
    private static string secondFemaleGenetive = "двух";
    private static string[] from3till19 = new string[18]{ "", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
    private static string[] from3till19Genetive = new string[18]{ "", "трех", "четырех", "пяти", "шести", "семи", "восеми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати" };
    private static string[] tens = new string[9]{ "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
    private static string[] tensGenetive = new string[9]{ "", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяноста" };
    private static string[] hundreds = new string[10]{ "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
    private static string[] hundredsGenetive = new string[10]{ "", "ста", "двухсот", "трехсот", "четырехсот", "пятисот", "шестисот", "семисот", "восемисот", "девятисот" };
    private static string[] thousands = new string[4]{ "", "тысяча", "тысячи", "тысяч" };
    private static string[] thousandsAccusative = new string[4]{ "", "тысячу", "тысячи", "тысяч" };
    private static string[] millions = new string[4]{ "", "миллион", "миллиона", "миллионов" };
    private static string[] billions = new string[4]{ "", "миллиард", "миллиарда", "миллиардов" };
    private static string[] trillions = new string[4]{ "", "трилион", "трилиона", "триллионов" };
    private static string[] rubles = new string[4]{ "", "рубль", "рубля", "рублей" };
    private static string[] copecks = new string[4]{ "", "копейка", "копейки", "копеек" };

    public static string DateToTextLong(DateTime _date, string _year)
    {
      string format = "«{0}» {1} {2}";
      int num = _date.Day;
      string str1 = num.ToString("D2");
      string str2 = RuDateAndMoneyConverter.MonthName(_date.Month, TextCase.Genitive);
      num = _date.Year;
      string str3 = num.ToString();
      return string.Format(format, (object) str1, (object) str2, (object) str3) + (_year.Length != 0 ? " " : "") + _year;
    }

    public static string DateToTextLong(DateTime _date)
    {
      string format = "«{0}» {1} {2}";
      int num = _date.Day;
      string str1 = num.ToString("D2");
      string str2 = RuDateAndMoneyConverter.MonthName(_date.Month, TextCase.Genitive);
      num = _date.Year;
      string str3 = num.ToString();
      return string.Format(format, (object) str1, (object) str2, (object) str3);
    }

    public static string DateToTextQuarter(DateTime _date)
    {
      return RuDateAndMoneyConverter.NumeralsRoman(RuDateAndMoneyConverter.DateQuarter(_date)) + " квартал " + _date.Year.ToString();
    }

    public static string DateToTextSimple(DateTime _date)
    {
      return string.Format("{0:dd.MM.yyyy}", (object) _date);
    }

    public static int DateQuarter(DateTime _date)
    {
      return (_date.Month - 1) / 3 + 1;
    }

    private static bool IsPluralGenitive(int _digits)
    {
      return _digits >= 5 || _digits == 0;
    }

    private static bool IsSingularGenitive(int _digits)
    {
      return _digits >= 2 && _digits <= 4;
    }

    private static int lastDigit(long _amount)
    {
      long num = _amount;
      if (num >= 100L)
        num %= 100L;
      if (num >= 20L)
        num %= 10L;
      return (int) num;
    }

    public static string CurrencyToTxt(double _amount, bool _firstCapital)
    {
      long num = (long) Math.Floor(_amount);
      long _amount1 = (long) Math.Round(_amount * 100.0) % 100L;
      int _digits1 = RuDateAndMoneyConverter.lastDigit(num);
      int _digits2 = RuDateAndMoneyConverter.lastDigit(_amount1);
      string str1 = RuDateAndMoneyConverter.NumeralsToTxt(num, TextCase.Nominative, true, _firstCapital) + " ";
      string str2 = (!RuDateAndMoneyConverter.IsPluralGenitive(_digits1) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits1) ? str1 + RuDateAndMoneyConverter.rubles[1] + " " : str1 + RuDateAndMoneyConverter.rubles[2] + " ") : str1 + RuDateAndMoneyConverter.rubles[3] + " ") + string.Format("{0:00} ", (object) _amount1);
      return (!RuDateAndMoneyConverter.IsPluralGenitive(_digits2) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits2) ? str2 + RuDateAndMoneyConverter.copecks[1] + " " : str2 + RuDateAndMoneyConverter.copecks[2] + " ") : str2 + RuDateAndMoneyConverter.copecks[3] + " ").Trim();
    }

    public static string CurrencyToTxtFull(double _amount, bool _firstCapital)
    {
      long num = (long) Math.Floor(_amount);
      long _amount1 = (long) Math.Round(_amount * 100.0) % 100L;
      int _digits1 = RuDateAndMoneyConverter.lastDigit(num);
      int _digits2 = RuDateAndMoneyConverter.lastDigit(_amount1);
      string str1 = string.Format("{0:N0} ({1}) ", (object) num, (object) RuDateAndMoneyConverter.NumeralsToTxt(num, TextCase.Nominative, true, _firstCapital));
      string str2 = (!RuDateAndMoneyConverter.IsPluralGenitive(_digits1) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits1) ? str1 + RuDateAndMoneyConverter.rubles[1] + " " : str1 + RuDateAndMoneyConverter.rubles[2] + " ") : str1 + RuDateAndMoneyConverter.rubles[3] + " ") + string.Format("{0:00} ", (object) _amount1);
      return (!RuDateAndMoneyConverter.IsPluralGenitive(_digits2) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits2) ? str2 + RuDateAndMoneyConverter.copecks[1] + " " : str2 + RuDateAndMoneyConverter.copecks[2] + " ") : str2 + RuDateAndMoneyConverter.copecks[3] + " ").Trim();
    }

    public static string CurrencyToTxtShort(double _amount, bool _firstCapital)
    {
      long _amount1 = (long) Math.Floor(_amount);
      long _amount2 = (long) Math.Round(_amount * 100.0) % 100L;
      int _digits1 = RuDateAndMoneyConverter.lastDigit(_amount1);
      int _digits2 = RuDateAndMoneyConverter.lastDigit(_amount2);
      string str1 = string.Format("{0:N0} ", (object) _amount1);
      string str2 = (!RuDateAndMoneyConverter.IsPluralGenitive(_digits1) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits1) ? str1 + RuDateAndMoneyConverter.rubles[1] + " " : str1 + RuDateAndMoneyConverter.rubles[2] + " ") : str1 + RuDateAndMoneyConverter.rubles[3] + " ") + string.Format("{0:00} ", (object) _amount2);
      return (!RuDateAndMoneyConverter.IsPluralGenitive(_digits2) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits2) ? str2 + RuDateAndMoneyConverter.copecks[1] + " " : str2 + RuDateAndMoneyConverter.copecks[2] + " ") : str2 + RuDateAndMoneyConverter.copecks[3] + " ").Trim();
    }

    private static string MakeText(int _digits, string[] _hundreds, string[] _tens, string[] _from3till19, string _second, string _first, string[] _power)
    {
      string str = "";
      int num = _digits;
      if (num >= 100)
      {
        str = str + _hundreds[num / 100] + " ";
        num %= 100;
      }
      if (num >= 20)
      {
        str = str + _tens[num / 10 - 1] + " ";
        num %= 10;
      }
      if (num >= 3)
        str = str + _from3till19[num - 2] + " ";
      else if (num == 2)
        str = str + _second + " ";
      else if (num == 1)
        str = str + _first + " ";
      if (_digits != 0 && (uint) _power.Length > 0U)
      {
        int _digits1 = RuDateAndMoneyConverter.lastDigit((long) _digits);
        str = !RuDateAndMoneyConverter.IsPluralGenitive(_digits1) ? (!RuDateAndMoneyConverter.IsSingularGenitive(_digits1) ? str + _power[1] + " " : str + _power[2] + " ") : str + _power[3] + " ";
      }
      return str;
    }

    public static string NumeralsToTxt(long _sourceNumber, TextCase _case, bool _isMale, bool _firstCapital)
    {
      string str = "";
      long num1 = _sourceNumber;
      int num2 = 0;
      if (num1 >= (long) Math.Pow(10.0, 15.0) || num1 < 0L)
        return "";
      while (num1 > 0L)
      {
        int _digits = (int) (num1 % 1000L);
        num1 /= 1000L;
        switch (num2)
        {
          case 9:
            str = RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, RuDateAndMoneyConverter.secondMale, RuDateAndMoneyConverter.firstMale, RuDateAndMoneyConverter.billions) + str;
            break;
          case 12:
            str = RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, RuDateAndMoneyConverter.secondMale, RuDateAndMoneyConverter.firstMale, RuDateAndMoneyConverter.trillions) + str;
            break;
          case 3:
            str = _case == TextCase.Accusative ? RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, RuDateAndMoneyConverter.secondFemale, RuDateAndMoneyConverter.firstFemaleAccusative, RuDateAndMoneyConverter.thousandsAccusative) + str : RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, RuDateAndMoneyConverter.secondFemale, RuDateAndMoneyConverter.firstFemale, RuDateAndMoneyConverter.thousands) + str;
            break;
          case 6:
            str = RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, RuDateAndMoneyConverter.secondMale, RuDateAndMoneyConverter.firstMale, RuDateAndMoneyConverter.millions) + str;
            break;
          default:
            string[] _power = new string[0];
            switch (_case)
            {
              case TextCase.Genitive:
                str = RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundredsGenetive, RuDateAndMoneyConverter.tensGenetive, RuDateAndMoneyConverter.from3till19Genetive, _isMale ? RuDateAndMoneyConverter.secondMaleGenetive : RuDateAndMoneyConverter.secondFemaleGenetive, _isMale ? RuDateAndMoneyConverter.firstMaleGenetive : RuDateAndMoneyConverter.firstFemale, _power) + str;
                break;
              case TextCase.Accusative:
                str = RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, _isMale ? RuDateAndMoneyConverter.secondMale : RuDateAndMoneyConverter.secondFemale, _isMale ? RuDateAndMoneyConverter.firstMale : RuDateAndMoneyConverter.firstFemaleAccusative, _power) + str;
                break;
              default:
                str = RuDateAndMoneyConverter.MakeText(_digits, RuDateAndMoneyConverter.hundreds, RuDateAndMoneyConverter.tens, RuDateAndMoneyConverter.from3till19, _isMale ? RuDateAndMoneyConverter.secondMale : RuDateAndMoneyConverter.secondFemale, _isMale ? RuDateAndMoneyConverter.firstMale : RuDateAndMoneyConverter.firstFemale, _power) + str;
                break;
            }
            break;
        }
        num2 += 3;
      }
      if (_sourceNumber == 0L)
        str = RuDateAndMoneyConverter.zero + " ";
      if (str != "" & _firstCapital)
        str = str.Substring(0, 1).ToUpper() + str.Substring(1);
      return str.Trim();
    }

    public static string NumeralsDoubleToTxt(double _sourceNumber, int _decimal, TextCase _case, bool _firstCapital)
    {
      long _sourceNumber1 = (long) Math.Round(_sourceNumber * Math.Pow(10.0, (double) _decimal)) % (long) Math.Pow(10.0, (double) _decimal);
      return string.Format(" {0} целых {1} сотых", (object) RuDateAndMoneyConverter.NumeralsToTxt((long) _sourceNumber, _case, true, _firstCapital), (object) RuDateAndMoneyConverter.NumeralsToTxt(_sourceNumber1, _case, true, false)).Trim();
    }

    public static string MonthName(int _month, TextCase _case)
    {
      string str = "";
      if (_month > 0 && _month <= 12 && _case == TextCase.Genitive)
        str = RuDateAndMoneyConverter.monthNamesGenitive[_month];
      return str;
    }

    public static string NumeralsRoman(int _number)
    {
      string str = "";
      switch (_number)
      {
        case 1:
          str = "I";
          break;
        case 2:
          str = "II";
          break;
        case 3:
          str = "III";
          break;
        case 4:
          str = "IV";
          break;
      }
      return str;
    }
  }
}
