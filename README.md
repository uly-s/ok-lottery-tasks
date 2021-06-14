# ok-lottery-tasks

Solved on the assumption that 'counting combinations' refers to literal combinatoric combinations or nCr "pick r out of n items without repetition", helper functions in Helpers.cs keep the Main method in Program.cs to 4 function calls.

Eye kept on "what if this had to count arbitrarily sized combinations" and "what if the relevant games changed" as well as scaling complexity (small data set but I don't sleep on factorials). I like to think in terms of generalized versus hardcoded and fragile versus robust so I made it to where you can drop in any json file of the same format and get the count/combinations. 
