MODULE TestExp ;

CONST
	lowest = 3;
	hi = 7;
	middle = 7;

VAR
   good, bad, ugly : INTEGER;

BEGIN

    good := 42+7-31+1 - 14;

    WRSTR ("Testing expressions:");
    WRLN;
    WRSTR ("42+7-31+1: should be 19: ");
    WRINT (42+7-31+1);
    WRLN;
    WRSTR ("good * lowest: should be 15: ");
    WRINT (good * lowest);
    WRLN;
    WRSTR ("-good * lowest: should be -15: ");
    WRINT (-good * lowest);
    WRLN;
    WRSTR ("42000 DIV 4200: should be 10: ");
    WRINT (42000 DIV 4200);
    WRLN;
    WRSTR ("42042 MOD 100: should be 42: ");
    WRINT (42042 MOD 100);
    WRLN;
    WRSTR ("20000 * 21000: should be 420,000,000: ");
    WRINT (20000 * 21000);
    WRLN;
    WRSTR ("-20000 * 21000: should be -420,000,000: ");
    WRINT (-20000 * 21000);
    WRLN;
    WRSTR ("6*7+2*lowest-1: should be 47: ");
    WRINT (6*7+2*lowest-1);
    WRLN;
    WRSTR ("6*2+((1+2)*3-1)*2: should be 28: ");
    WRINT (6*2+((1+2)*3-1)*2);
    WRLN;
END TestExp.