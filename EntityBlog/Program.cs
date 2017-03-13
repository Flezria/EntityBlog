using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace EntityBlog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.Create Blog\n2.CreatePost\n3.List All blogs\n4.List specific blog\n5.Edit a post\n6.Delete a blog or post\n0.End\nPlease enter your choice:");
            int choice = int.Parse(Console.ReadLine());
            Console.Clear();
            while (choice != 0)
            {
                switch (choice)
                {
                    case 1:
                        CreateBlog();
                        break;
                    case 2:
                        CreatePost();
                        break;
                    case 3:
                        ListAllBlogs();
                        break;
                    case 4:
                        ListSpecificBlog();
                        break;
                    case 5:
                        EditPost();
                        break;
                    case 6:
                        DeleteElement();
                        break;
                }
                Console.WriteLine(
                    "1.Create Blog\n2.CreatePost\n3.List All blogs\n4.List specific blog\n5.Edit a post\n6.Delete a blog or post\n0.End\nPlease enter your choice:");
                choice = int.Parse(Console.ReadLine());
                Console.Clear();
            }
            Environment.Exit(0);

        }


        private static void CreateBlog()
        {
            using (var dbObj = new BlogContext())
            {
                Console.Write("What is your username?: ");
                String username = Console.ReadLine();
                Console.Write("Write a name for a new blog: ");
                String name = Console.ReadLine();
                Console.Write("Write a url for a new blog: ");
                String url = Console.ReadLine();

                Blogs blog = new Blogs { UserName = username, Name = name, Url = url };
                dbObj.Blogs.Add(blog);
                dbObj.SaveChanges();

                Console.Clear();

                var query = from b in dbObj.Blogs
                            orderby name
                            select b;

                Console.WriteLine("All blogs in Database: ");
                foreach (var item in query)
                {
                    Console.WriteLine($"Username: {item.UserName} | Blog name: {item.Name} | Blog url: {item.Url}");
                }
            }

            Console.WriteLine("Press any key to return..");
            Console.ReadKey();
            Console.Clear();
        }
        private static void CreatePost()
        {
            using (var dbObj = new BlogContext())
            {
                Console.Write("Write a title for a new post: ");
                String posttitle = Console.ReadLine();
                Console.Write("Write content for the new post: ");
                String postcontent = Console.ReadLine();
                Console.Write("Write blog id: ");
                String blogid = Console.ReadLine();

                int output;
                Int32.TryParse(blogid, out output);

                Posts post = new Posts { Title = posttitle, Content = postcontent, BlogId = output };
                dbObj.Posts.Add(post);
                dbObj.SaveChanges();

                Console.Clear();

                var query = from p in dbObj.Posts
                            where p.BlogId.Equals(output)
                            orderby p.Title
                            select p;

                Console.WriteLine($"All posts for the blog with the id {blogid}: ");
                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("Press any key to return..");
            Console.ReadKey();
            Console.Clear();
        }
        private static void ListAllBlogs()
        {
            /*
            using (var dbObj = new BlogContext())
            {
                var query = from b in dbObj.Blogs
                            orderby b.BlogId
                            select b;

                foreach (var item in query)
                {
                    Console.WriteLine($"{item.BlogId}, {item.Name} | {item.Url}");

                    //Henter alle posts fra blogid og laver det til en liste
                    var posts = dbObj.Posts.Where(x => x.BlogId == item.BlogId).ToList();
                    //Printer posts
                    foreach (var post in posts)
                    {
                        Console.WriteLine($" Title: {post.Title}\r\n Content: {post.Content}");
                    }

                }
                Console.ReadKey();
            }
            */

            //Den rigtige løsning
            using (var dbObj = new BlogContext())
            {
                var query = dbObj.Blogs.Include(x => x.Posts).ToList();

                foreach (var item in query)
                {
                    Console.WriteLine($"{item.UserName}({item.BlogId}), {item.Name} | {item.Url}");
                    Console.WriteLine($"{ string.Join("\r\n", item.Posts) } \n");
                }

                Console.WriteLine("Press any key to return..");
                Console.ReadKey();
                Console.Clear();
            }

        }

        private static void ListSpecificBlog()
        {
            using (var dbObj = new BlogContext())
            {
                Console.Write("Indtast et blogid: ");
                String blogid = Console.ReadLine();

                int output;
                Int32.TryParse(blogid, out output);

                var query = dbObj.Blogs.Where(x => x.BlogId == output).Include(y => y.Posts).ToList();

                foreach (var item in query)
                {
                    Console.WriteLine($"{item.UserName}({item.BlogId}), {item.Name} | {item.Url}");
                    Console.WriteLine($"{ string.Join("\r\n", item.Posts) } \n");
                }

                Console.WriteLine("Press any key to return..");
                Console.ReadKey();
                Console.Clear();

            }
        }

        private static void EditPost()
        { 
            using(var dbObj = new BlogContext()) {
            Console.Write("Write ID on the post you want to edit: ");
            String postid = Console.ReadLine();

            int output;
            Int32.TryParse(postid, out output);

            var original = dbObj.Posts.Find(output);
            Console.WriteLine(original);

                Console.WriteLine("\nWhat do you want to edit?\n1.Post title\n2.Post content\n3.Both\nPlease enter your choice:");
                String choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("New title: ");
                        String title = Console.ReadLine();
                        original.Title = title;
                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("New content: ");
                        String content = Console.ReadLine();
                        original.Content = content;
                        break;
                    case "3":
                        Console.Clear();
                        Console.Write("New title: ");
                        String titleCase3 = Console.ReadLine();
                        Console.Write("\nNew content: ");
                        String contentCase3 = Console.ReadLine();
                        original.Title = titleCase3;
                        original.Content = contentCase3;
                        break;
                    default:
                        Console.WriteLine("Wrong command");
                        break;
                }

                dbObj.SaveChanges();
                Console.Clear();

                var query2 = dbObj.Posts.Where(x => x.PostId == output);

                Console.WriteLine("Post after update:\n");

                foreach (var item in query2)
                {
                    Console.WriteLine(item);
                }


                Console.WriteLine("Press any key to return..");
                Console.ReadKey();
                Console.Clear();

            }
        }

        private static void DeleteElement()
        {
            using(var dbObj = new BlogContext()) {
                Console.WriteLine("What do you want to delete?\n1.Blog\n2.Post\n");
                String choice = Console.ReadLine();

                switch(choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Write ID on the blog you want to delete: ");
                        String blogid = Console.ReadLine();

                        int outputBlogId;
                        Int32.TryParse(blogid, out outputBlogId);

                        var blogForDelete = dbObj.Blogs.Find(outputBlogId);
                        Console.WriteLine($"\n{blogForDelete.UserName}({blogForDelete.BlogId}), {blogForDelete.Name} | {blogForDelete.Url}\n");

                        Console.Write("Are you sure you want to delete this blog and all its posts?:\n1.Yes\n2.No\n");
                        String confirmation = Console.ReadLine();

                        if(confirmation.Equals("1"))
                        {
                            dbObj.Blogs.Remove(blogForDelete);
                            Console.WriteLine("The blog has been deleted!");
                        } else
                        {
                            Console.WriteLine("Deletion was not confirmed!");
                        }

                        break;
                    case "2":
                        Console.Clear();
                        Console.Write("Write ID on the post you want to delete: ");
                        String postid = Console.ReadLine();

                        int outputPostId;
                        Int32.TryParse(postid, out outputPostId);

                        var postForDelete = dbObj.Posts.Find(outputPostId);
                        Console.WriteLine($"\n{postForDelete}\n");

                        Console.Write("Are you sure you want to delete this post?:\n1.Yes\n2.No\n");
                        String confirmationPost = Console.ReadLine();

                        if (confirmationPost.Equals("1"))
                        {
                            dbObj.Posts.Remove(postForDelete);
                            Console.WriteLine("The post has been deleted!");
                        }
                        else
                        {
                            Console.WriteLine("Deletion was not confirmed!");
                        }
                        break;
                    default:
                        break;
                }
                dbObj.SaveChanges();
                Console.WriteLine("Press any key to return..");
                Console.ReadKey();
                Console.Clear();

            }
        }
    }
}
