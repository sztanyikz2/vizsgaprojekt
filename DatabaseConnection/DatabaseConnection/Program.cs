// See https://aka.ms/new-console-template for more information
using DatabaseConnection;
Console.WriteLine("Hello, World!");
model model = new model("https://localhost:7222/");
var users = await model.GetUsernamBySearch("john");
users.ForEach(user => Console.WriteLine($"User: {user.username}, Email: {user.useremail}"));
var posts = await model.GetPostsBySearch("hello");
posts.ForEach(post => Console.WriteLine($"Post ID: {post.postID}, Title: {post.title}"));
await model.FavouritePost(1);
Console.WriteLine("Post favourited successfully.");
await model.DeleteOwnPost(2);
Console.WriteLine("Own post deleted successfully.");
await model.ModerateComments(3);
Console.WriteLine("Comments moderated successfully.");
await model.DeletePost(4);
Console.WriteLine("Post deleted successfully.");
await model.ManageUsers(5);
Console.WriteLine("User managed successfully.");
await model.ModifyUsers(6);
Console.WriteLine("User modified successfully.");



