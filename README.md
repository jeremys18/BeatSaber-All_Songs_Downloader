# BeatSaber-All_Songs_Downloader
Downloads all BeatSaber songs from BeatSaver.com

Yes, I know, it's very non-flashy flashy. But this was designed for myself and I don't need flashy flashy. I don't actually think anyone else will use it, so if you're downloading this then I guess I was wrong. lol.


# How to Use

You need to decide how you want to store the song data FIRST. The app downloads all the song info from BeatSaver servers. To keep track of the songs you already downloaded it saves that info to a database, if you want, or to a file if you don't.

This isn't needed by you personally unless you want a way to have the songinfo at any time, but is needed by the app. For example, if you always want the ability to know what every song on the server is without having to actually check the server. But it's used by the app to know instantly what songs you have already gotten and eliminate them from redownload. It's also used in case you just want to redownload missing songs instead of getting the latest list from the server.

So first decide where to store the data. If you already have  ssms and a local db I'd very much suggest using the database option. If you don't but want to use the db option, suggested, then downloand and install SSMS and make sure the local DB option is checked during install. 

If you want to skip the whole db option then move to the next step.

1. Launch the app
2. input the number of songs to download at a time. 50 is the max. If you put anything more the app will override it to 50
3. click browse and chose a folder to save the songs to

### Options

#### Download Song Data

This tells the app to connect to the beat saver servers and grab the latest list of songs. Like...ALL of them. This is time consuming though as the servers have firewalls that limit the number of request the app can make to 90 at a time. Each request gets 10 or 15 (can't remeber) songs. WIth over 20,000 songs available it takes a lot of calls to get the data for all of them.

So use/check this option when you are wanting to a) get all the songs for the first time ever or b) you want to get the very latest list of songs.

Do NOT use/check this is you want to simply redownload any missing songs. So if you download all the songs then accidently delete 50 of them from your computer and you want to only get those 50 back, do NOT check this box as it will waist a lot of time getting the server info for all the songs.

#### Save to database

This is that file thing mentioned above. If you don't have ssms installed with the localdb running at the location . (open ssms and connect to .) then you don't want to check this box. Like ever. Because, ya know, you don't have any db server for it to use.

If you do have a local db and you can connect to it using ssms at location . than great! Check the box and all the song data will be saved to the local db under the BeatSaver database.

If you don't check the box the song information will still be saved but just to a json file in your appdata folder.


### Buttons

The next step is to choose what action you want. 

#### Check for new songs

If you just want a list of the songs you don't have downloaded yet then click the Check for new songs button. This doesn't download the actual songs but does list any songs you don't have downloaded in the new songs list to the left.

If you checked download song data then this button will download the latest song info from the servers then tell you any songs you don't already have. 

Otherwise, if that is unchecked, it will tell you any songs that are no longer in the folder. It will use the file/db of song information to check your folder and list any songs not found ("new songs").

If you click this button AND uncheck the download box AND you have never downloaded the song information (clicked download all) then you will get a message saying you can't do that. There's nothing for the app to do as you have no song information and no songs for it to compare to. It needs the song information from the server first. Once you have gotten all the song information at least ONCE , you can hit this button without checking the download data box.

###### Note

This button will save the latest song info to the db/file if the download box is checked.

#### Download All

This button does as it says. It downloads the actual songs.

If this is your first time using the app the you MUST have the download box checked. 

If this isn't your first run then you can uncheck the download box. If you do then it will act like the check for new songs button but actually get the songs from the server. It will not get the latest song info, saving you a lot of time, but will check your folder for missing songs that it knows about (aka it will pull the song info from the db/file and use that to find missing songs in your folder). It will then proceed to download those missing songs from the server.

If you check the download box then it will grab the latest list from the server then proceed to download ONLY the songs you don't already have downloaded.

If you first checked for new songs using the check for new songs button THEN dhit this button, it will NOT check the server again for the latest list of songs as it already did that when you got the list of new songs. Instead it will ONLY download the songs listed in the New Songs list on the left. This saves you from waiting all over for it to get the list of songs from the server (it already knows the latest list).
