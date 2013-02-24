MODULE LoopIf ;

CONST
	little = 3;
	hi = 7;
	middle = 7;

VAR
   good, bad, ugly : INTEGER;

VAR k, m : INTEGER ;

BEGIN

    good := 42+7-31+1 - 14;

    WRSTR ("Testing expressions (enter 1 and press enter to continue):");
	ugly := RDINT ();
    WRLN;
    WRSTR ("42+7-31+1:           should be 19:          ");
    WRINT (42+7-31+1);
    WRLN;
    WRSTR ("good * little:       should be 15:          ");
    WRINT (good * little);
    WRLN;
    WRSTR ("42000 DIV 4200:      should be 10:          ");
    WRINT (42000 DIV 4200);
    WRLN;
    WRSTR ("42042 MOD 100:       should be 42:          ");
    WRINT (42042 MOD 100);
    WRLN;
    WRSTR ("20000 * 21000:       should be 420,000,000: ");
    WRINT (20000 * 21000);
    WRLN;
    WRSTR ("6*7+2*little-1:      should be 47:          ");
    WRINT (6*7+2*little-1);
    WRLN;
    WRSTR ("6*2+((1+2)*3-1)*2:   should be 28:          ");
    WRINT (6*2+((1+2)*3-1)*2);
    WRLN;

    WRSTR ("IF test(5):          should be  4:          ");
        k := 4 ;
        m := 3 ;
        IF k > m THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE
            WRINT ( m ) ;
            WRLN ;
        END ;

    WRSTR ("IF test(5.1):        should be 17:          ");
        k := 4 ;
        m := 17 ;
        IF k > m THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE
            WRINT ( m ) ;
            WRLN ;
        END ;

    WRSTR ("IF test(5.2):        should be 71:          ");
        k := 4 ;
        m := 71 ;
        IF k > m THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE

            IF ( m <= 71 ) THEN
                WRINT ( m ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("AND test(5.3):       should be 21:          ");
        k := 4 ;
        m := 71 ;
        IF (k < m) AND ( (k*m) >= (k*m-1) ) THEN
            WRINT ( 21 ) ;
            WRLN ;
        ELSE

            IF m <= 71 THEN
                WRINT ( m ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("AND test(5.4):       should be 22:          ");
        k := 4 ;
        m := 71 ;
        IF (m > k) AND ( (k*m) >= (k*m-1) ) THEN
            WRINT ( 22 ) ;
            WRLN ;
        ELSE

            IF ( m <= 71 ) THEN
                WRINT ( m ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("OR test(5.5):        should be 23:          ");
        k := 4 ;
        m := 71 ;
        IF (k > k) OR (10 <> 10) THEN
            WRINT ( 1111 ) ;
            WRLN ;
        ELSE
            IF ( m <= 71 ) THEN
                WRINT ( 23 ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("OR test(5.6):        should be 24:          ");
    IF (5 > 3) OR ( k*m = (k*m-1) ) THEN
        WRINT ( 24 ) ; WRLN ;  END ;


    WRSTR ("OR AND test(5.7):    should be 25:          ");
    IF (5 < 3) OR ( k*m <> (k*m-1) ) AND (2 = 2) THEN
        WRINT ( 25 ) ; WRLN ;  END ;


    WRSTR ("LOOP test(6):       should be 1-9:        ");
        k := 10 ;
        m := 1 ;
        WRLN ;
        LOOP
            IF m = k THEN EXIT ; END ;
            WRINT ( m ) ;
            WRLN ;
            m := m + 1 ;
        END ;
        WRLN ;


    WRSTR ("LOOP test(6.1):     should be nice:       ");
        k := 10 ;
        m := 1 ;
        WRLN ;
        LOOP
            IF m = k THEN EXIT ; END ;

            good := 1;
            LOOP
                WRSTR ("@");
                IF good = m THEN EXIT ; END ;
                good := good + 1 ;
            END ;

            WRLN ;
            m := m + 1 ;
        END ;
        WRLN ;

    WRSTR ("Done testing expressions (enter 1 and press enter to continue):");
	ugly := RDINT ();

END LoopIf.