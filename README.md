# BeatSaber-All_Songs_Downloader
Downloads all BeatSaber songs from BeatSaver.com

Yes, I know, it's very non-flashy flashy. But this was designed for myself and I don't need flashy flashy. I don't actually think anyone else will use it, so if you're downloading this then I guess I was wrong. lol.

If you run into issue please open a new issue here. If you have any suggestions on how to make this better or things you'd like to see, open an issue here as well. I will look at them when I can.


# How to Use
 The app is very simple. It's entire goal is nothing more than to download EVERY song from the BeatSaver servers.
 
 This is a time consuming thing to do so don't expect to get evrything in 5 mins. That's the bad news.
The good news is that this app is now on ver 2.0! :D Why's that good? Well, because in ver 1 you had to wait hours for it to grab all the song information from the beatsaver servers. It takes 6 hours nowadays to do just that step. :O But in ver 2.0 I've split out the code to have our own server. My server does all the work for you so all this app has to do now is......call my server. It takes like 20 seconds, usualy like 5, to get what took hours before. So now the app gets all the info it needs quickly and then downloads the songs for you.

But ... it ... gets ... better!

In ver 1, everything relied on beatsaver servers. If their servers cchanged or went down, well, so did the entire app. If a song was removed from beatsaver servers, it was gone forever. If a song was on their server but for some reason couldn't be downloaded...gone. There was no backup method in case of failure.

But now there is! Because my server knows about every song AND downloads a copy of every song, there's a failsafe. If the app can't get the song from beatsaver servers it will now default to using my server as a backup. :D That means we no longer need to rely on their servers. If their server goes dowwn mine can still serve the files and the app still works.

## But how do you use it??

Well that's really simple. You launch it and then set the only options you can. Then just either download them all OR if you just want to see what songs are new, hit the "Check for new songs" button.

That's it! Very simple. The app will call my server and ask for a list of EVERY song. Then show you the list of new songs. If you only checked for new songs then it will not go any further till you hit the download all button. Otherwise it will move right along and download every song, alerting you of any errors along the way. If a song fails to download it'll show up in the second list.

### Options

#### Download Song Data

This tells the app to connect to my server and grab the list of songs. If you uncheck this (it's checked by default) you are telling the app you ONLY want to check for missing downloads. Aka, you deleted a song file and went OMG! I want it back! In that case you have no need to waste time grabbing ALL the song info from the server as you just want to ensure the songs are already downloaded and reget the missing songs.

So use/check this option when you are wanting to 
a) get all the songs for the first time ever or 
b) you want to get the very latest list of songs

Do NOT use/check this is you want to simply redownload any missing songs. So if you download all the songs then accidently delete 50 of them from your computer and you want to only get those 50 back, do NOT check this box as it will waist time getting the song info for all the songs.


### Buttons

The next step is to choose what action you want. 

#### Check for new songs

If you just want a list of the songs you don't have downloaded yet then click the Check for new songs button. This doesn't download the actual songs but does list any songs you don't have downloaded in the new songs list to the left.

If you checked download song data then this button will download the latest song info from the servers then tell you any songs you don't already have. 

Otherwise, if that is unchecked, it will tell you any songs that are no longer in the folder. It will use the local file of song information to check your folder and list any songs not found ("new songs").

If you click this button AND uncheck the download box AND you have never downloaded the song information (clicked download all) then you will get a message saying you can't do that. There's nothing for the app to do as you have no song information and no songs for it to compare to. It needs the song information from the server first. Once you have gotten all the song information at least ONCE, you can hit this button without checking the download data box.

###### Note

This button will save the latest song info to a local file if the download box is checked.

#### Download All

This button does as it says. It downloads the actual songs.

If this is your first time using the app the you MUST have the download box checked. 

If this isn't your first run then you can uncheck the download box. If you do then it will act like the check for new songs button but actually get the songs from the server. It will not get the latest song info, saving you a lot of time, but will check your folder for missing songs that it knows about (aka it will pull the song info from the local file and use that to find missing songs in your folder). It will then proceed to download those missing songs from the server.

If you check the download box then it will grab the latest list from the server then proceed to download ONLY the songs you don't already have downloaded.

If you first checked for new songs using the check for new songs button THEN hit this button, it will NOT check the server again for the latest list of songs as it already did that when you got the list of new songs. Instead it will ONLY download the songs listed in the New Songs list on the left. This saves you from waiting all over for it to get the list of songs from the server (it already knows the latest list).
