MODULE test07 ;

VAR k : INTEGER ;

PROCEDURE little
( i : INTEGER ) ;

VAR j : INTEGER ;

BEGIN
    j := i * 4 ;
    WRINT ( i ) ;
    WRSTR ( " " ) ;
    WRINT ( j ) ;
    WRLN ;
END little ;

BEGIN
    WRSTR ( "We should write a 3 and a 12: ") ;
    k := 3 ;
    little ( k ) ;
END test07.