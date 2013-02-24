MODULE sieve ;

CONST
    maxIntInSqrt = 8 ;

TYPE
    prListType = ARRAY [2 .. 50] OF INTEGER ;

VAR
    newSieve
        : prListType ;

    currPrime,
    mult,
    index
        : INTEGER ;

BEGIN
    index := 2 ;
    LOOP
        IF index > 50 THEN EXIT ; END ;
        newSieve[index] := 0 ;
        index := index + 1 ;
    END ;
    currPrime := 2 ;
    LOOP
        IF currPrime > maxIntInSqrt THEN EXIT ; END ;
        mult := 2 ;
        LOOP
            IF mult * currPrime > 50 THEN EXIT ; END ;
            newSieve [mult * currPrime] := 1 ;
            mult := mult + 1 ;
        END ;
        LOOP
            currPrime := currPrime + 1 ;
            IF newSieve[currPrime] = 0 THEN EXIT ; END ;
        END ;
    END ;
    index := 2 ;
    LOOP
        IF index > 50 THEN EXIT ; END ;
        IF newSieve[index] = 0 THEN
            WRINT ( index ) ;
            WRLN ;
        END ;
        index := index + 1 ;
    END ;

END sieve.