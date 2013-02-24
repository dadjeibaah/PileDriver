MODULE square;

TYPE
    prListType = ARRAY [11 .. 30] OF INTEGER ;

VAR
    newSieve
        : prListType ;

    currPrime,
    mult,
    index
        : INTEGER ;

BEGIN

	(* Load values into the array *)
    index := 11 ;
    LOOP
        IF index > 30 THEN EXIT ; END ;
        newSieve[index] := index * index;
        index := index + 1 ;
    END ;

	(* Print the array *)
    index := 11 ;
    LOOP
        IF index > 30 THEN EXIT ; END ;
        WRINT ( index ) ;
        WRSTR ( " squared is " ) ;
        WRINT ( newSieve[index] ) ;
        WRSTR ( "." ) ;
        WRLN ;
        index := index + 1 ;
    END ;

END square.