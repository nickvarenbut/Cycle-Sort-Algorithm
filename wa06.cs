using System;
using System.Diagnostics;
using System.Linq;

using static System.Console;

namespace Bme121
{
    static class Program
    {
        static Random rGen;

        // Create an integer array filled with random values.
        // This is a modified version which allows generation of an array
        // with no repeated values (all values unique).

        static int[ ] NewRandomArray( int size, bool unique = false )
        {
            if( size < 1 ) return new int[ 0 ];

            int[ ] result = new int[ size ];
            Array.Fill( result, -1 ); // Each -1 will be replaced by a nonnegative value

            int range;
            if( unique ) range = 4 * size / 3;
            else /* ( ! unique ) */ range = 3 * size / 4;

            for( int i = 0; i < result.Length; i ++ )
            {
                int value = rGen.Next( 0, range );
                if( unique )
                while( result.Contains( value ) )
                {
                    value = rGen.Next( 0, range );
                }
                result[ i ] = value;
            }

            return result;
        }

        // Write up to 10 elements of an integer array.
        // This version writes the array horizontally.

        static void WriteArray( int[ ] a, int fieldWidth = 0 )
        {
            if( a == null ) return;

            string fmt = " {0," + fieldWidth + "}";

            Write( "   i  " );
            if( a.Length >  0 ) Write( fmt, 0 );
            if( a.Length >  1 ) Write( fmt, 1 );
            if( a.Length >  2 ) Write( fmt, 2 );
            if( a.Length >  3 ) Write( fmt, 3 );
            if( a.Length >  4 ) Write( fmt, 4 );
            if( a.Length > 10 ) Write( " ..." );
            if( a.Length >  9 ) Write( fmt, a.Length - 5 );
            if( a.Length >  8 ) Write( fmt, a.Length - 4 );
            if( a.Length >  7 ) Write( fmt, a.Length - 3 );
            if( a.Length >  6 ) Write( fmt, a.Length - 2 );
            if( a.Length >  5 ) Write( fmt, a.Length - 1 );
            WriteLine( );

            Write( "a[ i ]" );
            if( a.Length >  0 ) Write( fmt, a[ 0 ] );
            if( a.Length >  1 ) Write( fmt, a[ 1 ] );
            if( a.Length >  2 ) Write( fmt, a[ 2 ] );
            if( a.Length >  3 ) Write( fmt, a[ 3 ] );
            if( a.Length >  4 ) Write( fmt, a[ 4 ] );
            if( a.Length > 10 ) Write( " ..." );
            if( a.Length >  9 ) Write( fmt, a[ a.Length - 5 ] );
            if( a.Length >  8 ) Write( fmt, a[ a.Length - 4 ] );
            if( a.Length >  7 ) Write( fmt, a[ a.Length - 3 ] );
            if( a.Length >  6 ) Write( fmt, a[ a.Length - 2 ] );
            if( a.Length >  5 ) Write( fmt, a[ a.Length - 1 ] );
            WriteLine( );
        }

        // Selection sort - sorts integers into increasing order.

        static void SelectionSort( int[ ] a )
        {
            if( a == null ) return;
            if( a.Length < 2 ) return;

            int reads = 0, writes = 0, compares = 0;

            for( int firstUnsorted = 0; firstUnsorted < a.Length - 1; firstUnsorted ++ )
            {
                int min = firstUnsorted;
                for( int i = firstUnsorted + 1; i < a.Length; i ++ )
                {
                    if( Compare( a[ i ], a[ min ] ) < 0 ) min = i;
                    compares++;
                    
                }

                int temp = a[ firstUnsorted ]; reads++;
                a[ firstUnsorted ] = a[ min ]; writes++;
                a[ min ] = temp; writes++;
                writes++;
                
            }

            WriteLine( "Reads = {0:n0}, writes = {1:n0}, compares = {2:n0}", reads, writes, compares );
        }

        // Insertion sort - sorts integers into increasing order.
        // To properly count reads and compares, I had to expand the test controlling the loop.

        static void InsertionSort( int[ ] a )
        {
            if( a == null ) return;
            if( a.Length < 2 ) return;

            int reads = 0, writes = 0, compares = 0;

            for( int firstUnsorted = 1; firstUnsorted < a.Length; firstUnsorted ++ )
            {
                int hole = firstUnsorted;
                reads ++;
                int temp = a[ hole ];

                bool notDone = hole > 0;
                if( notDone )
                {
                    
                    compares ++;
                    notDone = Compare( temp, a[ hole - 1 ] ) < 0;
                }
                while( notDone )
                {
                    
                    writes ++;
                    a[ hole ] = a[ hole -1 ];
                    hole = hole - 1;

                    notDone = hole > 0;
                    if( notDone )
                    {
                       
                        compares ++;
                        notDone = Compare( temp, a[ hole - 1 ] ) < 0;
                    }
                }

                writes ++;
                a[ hole ] = temp;
            }

            WriteLine( "Reads = {0:n0}, writes = {1:n0}, compares = {2:n0}", reads, writes, compares );
        }

        // Cycle sort - sorts integers into increasing order.

        static void CycleSort( int[ ] a )
        {
            if( a == null ) return;
            if( a.Length < 2 ) return;

            int reads = 0;
            int writes = 0;
            int compares = 0;

            // Find the number of values in 'a' that are less than 'value'.
            // In this accumulation, ignore the value at index 'skip'.

            int Rank( int value, int skip )
            {
                int result = 0;

                for( int i = 0; i < a.Length; i ++ )
                {
                    if( i != skip )
                    {
                        if( Compare( value, a[ i ] ) > 0 ) result ++;
                        compares++;
                        reads++;
                    }
                }

                return result;
            }

            for( int cycleStart = 0; cycleStart < a.Length; cycleStart ++ )
            {
                int item = a[ cycleStart ]; reads++;
                int position = Rank( item, skip: cycleStart );
                int temp=0;

                 bool cycleDone = false;
                 while( ! cycleDone )
                {
					int count =0;
                    if( position == cycleStart )
                     {
						
							a[cycleStart]=item;
							cycleDone = true;
							writes++;
							
						
                     }
                     else 
					 {
						 
						 if(a[position] == item && position<a.Length)
						 {
							 position ++;
							 reads++;
							 
						 }
						 
						else 
						{	 
							 temp =a[position]; reads++;
							 a[position] = item; writes++;
							 item=temp;
							 
							 position = Rank(item, skip: cycleStart);
						}
                     }
                 }
            }

            WriteLine( "Reads = {0:n0}, writes = {1:n0}, compares = {2:n0}", reads, writes, compares );
        }

        // Comparison method - returns -1, 0, +1 if 'a' is <, =, > 'b', respectively.

        static int Compare( int a, int b )
        {
            return a.CompareTo( b );
        }

        // Test sorting where all algorithms use the same random array.

        static void Main( )
        {
            int[ ] data;
            Stopwatch timer;
            const int seed = 1_111;
            // const int size = 10;
            const int size = 10_000;
            // const bool unique = true;
            const bool unique = false;
            const int fieldWidth = 5;

            // Test selection sort.

            rGen = new Random( seed );
            data = NewRandomArray( size, unique );

            WriteLine( );
            WriteLine( "Selection Sort, unsorted array" );
            WriteArray( data, fieldWidth );

            timer = new Stopwatch( );
            timer.Start( );
            SelectionSort( data );
            timer.Stop( );

            WriteLine( "Selection sort, sorted array" );
            WriteArray( data, fieldWidth );
            WriteLine( "Time = {0:f1} seconds", timer.Elapsed.TotalSeconds );

            // Test insertion sort.

            rGen = new Random( seed );
            data = NewRandomArray( size, unique );

            WriteLine( );
            WriteLine( "Insertion Sort, unsorted array" );
            WriteArray( data, fieldWidth );

            timer = new Stopwatch( );
            timer.Start( );
            InsertionSort( data );
            timer.Stop( );

            WriteLine( "Insertion sort, sorted array" );
            WriteArray( data, fieldWidth );
            WriteLine( "Time = {0:f1} seconds", timer.Elapsed.TotalSeconds );

            // Test cycle sort.

            rGen = new Random( seed );
            data = NewRandomArray( size, unique );

            WriteLine( );
            WriteLine( "Cycle Sort, unsorted array" );
            WriteArray( data, fieldWidth );

            timer = new Stopwatch( );
            timer.Start( );
            CycleSort( data );
            timer.Stop( );

            WriteLine( "Cycle sort, sorted array" );
            WriteArray( data, fieldWidth );
            WriteLine( "Time = {0:f1} seconds", timer.Elapsed.TotalSeconds );

            WriteLine( );
        }
    }
}
