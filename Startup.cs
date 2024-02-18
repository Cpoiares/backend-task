namespace BackendTask
{
    public class Startup
    {
        public static void Start(WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowLocalhost");
            app.UseIdentityServer();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
