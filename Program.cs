using System;
using static System.Console;
using System.Collections.Generic;
using System.IO;

namespace MinYoongisKPOPGroup
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Member> members = new List<Member>();
            FileReader fileReader = new FileReader();
            Activity activities = new Activity();

            string input = "";
            members = fileReader.ReadMemberFile("BTS.txt");
            Group group = fileReader.ReadGroupFile("groups.txt");

            

            //game-y stuff

            WriteLine("Welcome!");
            WriteLine("Your group is {0}", group.Name);

            while(input != "exit")
            {
                WriteLine("What would you like to do?");
                input = ReadLine();
                switch (input)
                {
                    case "command":
                        activities.Commands();
                        break;
                    case "train":
                        activities.Train(members);
                        break;
                    case "preform":
                        activities.Preform();
                        break;
                    case "make song":
                        group.songToAdd = activities.MakeSong();
                        break;
                    case "members":
                        activities.GetMemberInfo(group, members);
                        break;
                    case "songs":
                        activities.Songs(group);
                        break;
                    default:
                        WriteLine("Goodbye!");
                        break;

                }
            }

            Read();
        }
    }

    class Member
    {
        enum SpecialTrait { Charming, Brave, Thoughtful, Assertive, Perceptive }
        enum Position { Vocal, Rapper, Dancer }

        string name;
        string birthday;
        SpecialTrait specialTrait;
        Position position;
        string hobby;

        public string Name { get { return name; } }

        Dictionary<string, int> skills = new Dictionary<string, int>()
        {   {"rap", 0 },
            {"dance", 0 },
            {"vocal", 0 }
        };

        public Member(string memName, string memBirthday, int specialTraitInt,
            int pos, string memHobby, int rap, int dance, int vocal)
        {
            name = memName;
            birthday = memBirthday;
            specialTrait = (SpecialTrait)specialTraitInt;
            position = (Position)pos;
            hobby = memHobby;
            skills["rap"] = rap;
            skills["dance"] = dance;
            skills["vocal"] = vocal;
        }


        public void IncreaseSkill(string skillName)
        {
            skills[skillName]++;
            if (skills[skillName] > 100)
            {
                skills[skillName] = 100;
                WriteLine(name + " cannot level up " + skillName + " anymore.");
            }
            WriteLine(name + " has leveled up " + skillName +
                " to " + skills[skillName]);
        }

        public override string ToString()
        {
            return name + "\n——————" + "\nBirthday: " + birthday + "\nSpecial Trait: " + specialTrait.ToString() +
                "\nPosition: " + position.ToString() + "\nHobby: " +
                hobby + "\n——————" + "\nRap: " + skills["rap"] +
                "\nDance: " + skills["dance"] + "\nVocal: " +
                skills["vocal"];
        }

    }

    class Group
    {
        string name;
        List<Song> songs = new List<Song>();
        string debutDate;
        int numOfPreformances = 0;
        int fans;


        public List<Song> Songs { get { return songs; } }
        public Song songToAdd  { set { songs.Add(value); } }
        public string Name { get { return name; }  }

        public Group(string groupName, string groupDebut, int preformance,
            int fandom)
        {
            name = groupName;
            //songs = groupSongs;
            debutDate = groupDebut;
            numOfPreformances = preformance;
            fans = fandom;
        }

        public override string ToString()
        {
            return name + "\n——————" + "\nDebut Date: " + debutDate +
                "\nSongs: " + songs.Count + "\nPreformances: " + numOfPreformances
                + "\nFans: " + fans;
        }
    }

    class FileReader
    {
        //string path = "BTS.txt";
        string[] data = new string[8];

        List<Member> members = new List<Member>();


        public List<Member> ReadMemberFile(string path)
        {

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.EndOfStream != true)
                    {
                        data = sr.ReadLine().Split(';');
                        Member member = new Member(data[0], data[1],
                            int.Parse(data[2]), int.Parse(data[3]),
                            data[4], int.Parse(data[5]),
                            int.Parse(data[6]), int.Parse(data[7]));
                        members.Add(member);
                    }
                    sr.Close();
                }

            }
            else
                WriteLine("File does not exist");

            return members;
        }

        public Group ReadGroupFile(string path)
        {
            Group group = null;
          
            using(StreamReader sr = new StreamReader(path))
            {
                if (File.Exists(path))
                {
                    while(sr.EndOfStream != true)
                    {
                        data = sr.ReadLine().Split(';');
                        group = new Group(data[0], data[1], int.Parse(data[2]),
                            int.Parse(data[3]));
                    }
                }
            }
            return group;
        }

    }

    struct Song
    {
        enum Concept { Cute, Badass, Story, Sexy, Horror, Art }
        enum SongType { Pop, Ballad, Rock, Rap, EDM }

        string name;
        Concept concept;
        SongType songType;
        string lyrics;

        public Song(string songName, int conceptInt, int songTypeInt, string songLyrics)
        {
            name = songName;
            concept = (Concept)conceptInt;
            songType = (SongType)songTypeInt;
            lyrics = songLyrics;
        }

        public override string ToString()
        {
            return name + "\n——————" + "\nConcept: " + concept.ToString() +
                "\nGenre: " + songType.ToString() + "\nLyrics: " + lyrics;
        }
    }

    
    class Activity
    {

        public void GetMemberInfo(Group group, List<Member> members)
        {
            WriteLine("\n" + group.ToString() + "\n");
            foreach (Member member in members)
            {
                WriteLine("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
                WriteLine("\n" + member.ToString() + "\n");
                if (member == members[members.Count - 1])
                    WriteLine("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
            }
        }

        public void Commands()
        {
            WriteLine("exit - to exit");
            WriteLine("members - pulls up member infor  ");
            WriteLine("train - to train group members");
            WriteLine("make song - to make a song");
            WriteLine("songs - to see list of songs");
            WriteLine("preform - to preform");
        }

        public void Train(List<Member> members)
        {
            
            WriteLine("Please enter the number for the member you want to train: ");
            WriteLine("Kim Seokjin = 0, Min Yoongi = 1, Jung Hoseok = 2, Kim Namjoon = 3, Park Jimin = 4, Kim Taehyung = 5, Jeon Jungkook = 6");
            int memberNum = int.Parse(ReadLine());
            WriteLine("Please enter the skill you want to train {0}", members[memberNum].Name);
            WriteLine("You can choose from: rap, vocal, dance");
            string skill = ReadLine();
            members[memberNum].IncreaseSkill(skill);
        }

        public Song MakeSong()
        {
            string songName, lyrics;
            int concept, songType;

            //all wl and rl for making a song
            WriteLine("Please enter a name for the song: ");
            songName = ReadLine();
            WriteLine("Please enter a concept number: ");
            WriteLine(" Cute = 0, Badass = 1, Story = 2, Sexy = 3, Horror = 4, Art = 5");
            concept = int.Parse(ReadLine());
            WriteLine("Please enter a genre: ");
            WriteLine("Pop = 0, Ballad = 1, Rock = 2, Rap = 3, EDM = 4");
            songType = int.Parse(ReadLine());
            WriteLine("Please enter some lyrics: ");
            WriteLine("They can be anything you want!");
            WriteLine("They just have to sound like a general hook of a song.");
            lyrics = ReadLine();

            return new Song(songName, concept, songType, lyrics);
            //can release songs and albums. 
        }

        public void Songs(Group group)
        {
            foreach(Song song in group.Songs)
            {
                WriteLine("\n" + song.ToString() + "\n");
                WriteLine("-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
            }
        }

        public void Preform()
        {
            //check if group has songs
            //then ask what concert want to preform at. 
        }
    }
}
