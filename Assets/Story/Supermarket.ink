VAR checked_bed = false
VAR checked_desk = false
VAR changed_clothes = false
VAR checked_lamp = false
VAR current_page = 0
VAR guess_again = true
VAR found_matches = false
VAR examined_lamp = false

-> Introduction
== Introduction ==  

A low, piercing ring fills the room. The sound wakes you up, annoyed and tired. #Sound/Amber
You reach for your phone, the source of your disturbance, but before you can open it, the sound stops and the notification vanishes. You stare blankly at the screen for a bit before turning it on. 23:30. Hm. Now's as good of a time as any. You rub your eyes and stretch before getting up. #continue #Kill/Amber

* [Click here to continue...]
    -> Grocery_List_Hunting
    
== Grocery_List_Hunting ==
This apartment has seen better days and better renters. It stands quite bare, save for a few necessities; TV, fridge, a bed, and a desk are all that's there. You're not the cleanest, either; stains are splattered all across the carpet. And the smell - was it always this bad? You look around, searching for your grocery list. #Sound/Home
-> Options

= Options 
    + [Check near the bed]
        -> Check_Bed
    + [Check around the desk]
        -> Check_Desk
        
= Grocery_List_Abridged
    Where did you put it? You keep looking. #continue
    -> Options
    
= Check_Bed
    { - checked_bed:
        You find nothing else. 
        -> Grocery_List_Abridged
      - else:
        ~checked_bed = true
        { - checked_desk:
            You spot the paper near the edge of the nightstand and grab it. 
            ->Preparing_To_Leave
          - else: 
            You look all around the nightstand, but find nothing. Well, that's not entirely true - you find your wallet, carelessly thrown on the floor.
            -> Grocery_List_Abridged
        } 
    }
    
= Check_Desk
    { - checked_desk:
        You find nothing else. 
        -> Grocery_List_Abridged
      - else:
        ~checked_desk = true
        { - checked_bed:
            You find the paper under a paperweight and grab it.
            ->Preparing_To_Leave
          - else: 
            You look all around the desk, but find nothing. Well, that's not entirely true - you find your wallet, carelessly thrown on the floor.
            -> Grocery_List_Abridged
        } 
    }
    
== Preparing_To_Leave == 
You look at the list: bleach, cheese, pasta, some meat - right, you're running out of things to eat. The supermarket's not far; you start getting ready to head out. #continue
->Options

= Options
    + [Take a shower.]
        ->Try_Showering
    * [Change clothes.]
        ->Change_Clothes
    + [Walk outside.]
        ->Try_Heading_Out
    
= Preparing_Abridged_One 
    Time to change and head out. #continue
    ->Options

= Preparing_Abridged_Two 
    Time to head out. #continue
    ->Options

= Try_Showering
    The shop will close soon. You should shower later. 
    { - changed_clothes:
        -> Preparing_Abridged_Two
      - else:
        -> Preparing_Abridged_One
    }
    
= Change_Clothes
    You take off the filth that you were wearing and put on some clean clothes. Much better. You're sure to get fewer stares from impolite strangers. 
    ~changed_clothes = true
    -> Preparing_Abridged_Two

= Try_Heading_Out 
    { - changed_clothes:
        You open the door and head out. 
        * [Click here to continue... ]
            -> Crowded_Street
            
      - else: 
        {You look and smell terrible. Perhaps you should change.|You'd rather not offend the senses of those around you.|You'd rather not... |Maybe... |Maybe... |Maybe... it's for the best... |You take a deep breath and head out. ->Hack}
        ->Preparing_Abridged_One
        
    }
    
= Hack
    * [Click here to continue...]
        -> Crowded_Street

== Crowded_Street == 

On your way there, you spot a crowd gathering around a broken lamppost in the distance. It's too dark for you to see what they're doing or hear what they're saying, but you can make out the sounds of someone crying. #Kill/Home
-> Options 

= Options 
    + [Approach the crowd. ]
        -> Attempt_Approach
    + [Continue walking. ]
        -> Ignore
    
= Crowded_Abridged 
    You continue watching the crowd, motionless. #continue
    -> Options

= Attempt_Approach
    {The supermarket will close soon. Wasting time here might cost you.|Could it be - you hesitate. You would rather not know. You should continue on your way.|You shake your head. No, you <i>would</i> rather know. You start walking towards the crowd. ->Approach}
    ->Crowded_Abridged
    
= Approach 
    { - changed_clothes:
        Almost as if sensing your intentions, the crowd disperses. You spot, however, the figure of the crying women, as she steps inside one of the houses. Very quickly, you're left alone on the street. You look at the lamppost where the crowd was standing, but it's too dark to see anything - and you're not willing to move any closer. You turn around and continue towards the supermarket. #continue
            ~checked_lamp = true
            * [Click here to continue...]
                -> Supermarket
        
      - else:
        Almost as if sensing your intentions, the crowd disperses. Suddenly, however, you're blinded by a bright light; someone's pointed a flashlight towards you. Screams fill the air, and you feel your ribs cracking as you're brutally tackled to the ground. Your head slams against the street, and everything goes dark. #continue
        * [Click here to continue... ]
            -> END_THREE
    }

= Ignore 
    You ignore the crowd and continue on your way. 
    * [Click here to continue...]
        -> Supermarket
    
== Supermarket == 
You step inside. Looks like you barely made it; the clerk is getting ready to close up for the night. He looks up. As expected, he's piss drunk.

{ - changed_clothes: 
    "Hel-hel-hi the-re", he stammers, "We're . . ." You ignore him and start going through your list. #continue
  - else:
    "Hel-hel-hi the-re", he stammers, "Nic-e shirt". You stop, annoyed that he noticed despite his usual stupor. You double-check that there aren't any cameras before begin starting through your list. #continue
}

{ - checked_lamp:
    -> Forgot
  - else:
    * [Click here to continue...]
        -> Going_Back
}

= Forgot
    As you're about to leave, though, you stop. Aren't you forgetting something? You think for a moment. Yes, you're definitely forgetting something; something that you didn't need earlier. But what . . . ? #continue
    -> Menu

= Forgot_Abridged
    You pause and try and to remember what it was that you needed. #continue
    ->Menu

= Forgot_Abridged_Erase
    You pause and try and to remember what it was that you needed.
    ->Menu
    
= Menu    
    { - current_page == 0:
        * [Gloves.]
            -> Bad_Guess
        * [Matches.]
            -> Good_Guess
        * [Screwdiver.]
            -> Bad_Guess
        + [Next. ]
            ~current_page = 1
            -> Forgot_Abridged_Erase
      - else: 
        * [Shoelaces.]
            -> Bad_Guess
        * [Tape.]
            -> Bad_Guess
        * [Jerky.]
            -> Bad_Guess
        + [Previous. ]
            ~current_page = 0
            -> Forgot_Abridged_Erase
    }

= Bad_Guess
    { - guess_again:
         No, that wasn't it.
         ~guess_again = false
         -> Menu 
      - else: 
         No, that wasn't it either. Forget it, you think. Whatever it was can wait. You grab your bags and head out. 
         * [Click here to continue...]
            -> Going_Back
    }
    
= Good_Guess
    ~found_matches = true
    Yes, that's it. You buy a box of matches along with your things and head out.
     * [Click here to continue...]
        -> Going_Back
     
->END

== Going_Back ==

{ - checked_lamp: 
    -> Think
  - else:
    On your way back, you notice that the crowd is still there. You shrug and continue moving.
    * [Click here to continue...]
        -> END_ONE
}

= Options

    + [Continue walking home. ]
        -> Go_Home
    + [Examine the lamppost. ]
        -> Examine_Lamp
    + [Go back to the supermarket. ]
       -> Go_Back

= Think 
    You quickly reach the street where the crowd was gathered. You see nothing of note, save for the lamppost where they were standing. 
    -> Options

= Think_Abridged
    You stare at the lamppost, uncertain. #continue 
    -> Options

= Go_Back 
    You check the time and sigh. Too late.
    -> Think_Abridged

= Go_Home
    You shrug and continue on your way. 
    -> END_ONE
    
= Examine_Lamp

    { - examined_lamp:
        It's too dark to see what it is, and you're not getting any closer. 
        -> Think_Abridged
      - else :
        ~examined_lamp = true
        { - !found_matches: 
            You look around hesitantly and step towards the lamppost. Now that you're up close, you think you see something stuck to it, but it's too dark to tell. 
            -> Think_Abridged
          - else:
            You strike a match and bring it towards the lamppost.  You see a poster taped on, a girl's smiling face placed prominently in the center. You say nothing as you shake the matchstick and walk away. Now you know. 
            * [Click here to continue...]
                -> END_TWO
        }
    }

== END_ONE ==
Phew, you think as you lock the door, glad that's done with. You set your bags next to the fridge and relax on the bed. You look around the apartment and shake your head - perhaps it could use some cleaning. Especially the carpet. 
You stand up and get to work. #continue 
A low, piercing ring fills the room... #continue 
* [Click here to end the game...]
    -> END

== END_TWO ==
You make sure no one followed you as you lock the door behind you. Now was not the time to be careless. You sit on the bed as you ponder your next step. You need to be thorough . . . yes that's it. Erase everything. Might as well start now. 
You pick up the bleach. You open the fridge door. The stench of death is overpowering. #continue
Inspired by Shade and 9:05. #continue 
* [Click here to end the game...]
    -> END

== END_THREE ==
Hint: Perhaps something about you caught the crowd's attention . . .
* [Click here to end the game...]
    -> END