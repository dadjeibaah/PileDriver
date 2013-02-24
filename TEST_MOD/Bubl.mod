MODULE bubl ;

TYPE
    listType = ARRAY [ 1 .. 10 ] OF INTEGER ;

VAR
    thisList : listType ;

(****************************************)
PROCEDURE swap ( VAR a, b : INTEGER ) ;
VAR temp : INTEGER ;
BEGIN
    temp := a ;
    a    := b ;
    b    := temp ;
END swap ;

(****************************************)
PROCEDURE random ( VAR seed : INTEGER ) ;
CONST
    m = 8192 ;
    t = 4361 ;
    c = 3899 ;

VAR
    temp : INTEGER ;

BEGIN
    temp := ( seed * t + c ) MOD m ;
    seed := temp ;
END random ;

(*****************************************)
PROCEDURE fill ( VAR list : listType ) ;
CONST startSeed = 29 ;
VAR
    index,
    seed
        : INTEGER ;

BEGIN
    seed := startSeed ;
    index := 1 ;
    LOOP
        IF index > 10 THEN EXIT END ;
        list[index] := seed + 1000;
        random ( seed ) ;
        index := index + 1 ;
    END ;
END fill ;

(*****************************************)
PROCEDURE sort ( VAR list : listType ) ;
VAR
    index,
    mark
        : INTEGER ;

BEGIN
    LOOP
        mark := 0 ;
        index := 1 ;
        LOOP
            IF index > 9 THEN EXIT END ;
            IF list[index] > list[index+1] THEN
                swap ( list[index], list[index+1] ) ;
                mark := 1 ;
            END ;
            index := index + 1 ;
        END ;
        IF mark = 0 THEN EXIT END ;
    END ;
END sort ;

(*********************************************)
PROCEDURE print ( VAR list : listType ) ;
VAR
    index : INTEGER ;

BEGIN
    index := 1 ;
    LOOP
        IF index > 10 THEN EXIT END ;
        WRINT ( list[index] ) ;
        WRLN ;
        index := index + 1 ;
    END ;
END print ;

BEGIN
    fill  ( thisList ) ;
    print ( thisList ) ;
    WRLN ;
    sort  ( thisList ) ;
    print ( thisList ) ;
END bubl.