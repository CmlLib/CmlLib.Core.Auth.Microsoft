For using MsalClient, You should acquire your own Azure Client ID.  

## 1. Go to Azure Active Directory

Open [Azure Portal](https://portal.azure.com/) and find Azure Active Directory menu.

![image](https://user-images.githubusercontent.com/17783561/154854882-79918bb0-f317-4ab8-aac9-4b51f4086be9.png)

## 2. App registration

Add - App Registration

![image](https://user-images.githubusercontent.com/17783561/154855003-4f5fc4ea-7083-47f9-818d-72216a548c27.png)

Name: your app name  
Account type: Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)  
Redirect URI: Public client/native, http://localhost  

![image](https://user-images.githubusercontent.com/17783561/154855171-2198b328-9457-46e3-89b8-b1e295bfb5bd.png)

Click 'Register' button.

## 3. Authentication manage

Go to App registrations - your app name

![image](https://user-images.githubusercontent.com/17783561/154855363-17386531-4fb6-4fa3-aab2-3dc1ed954d48.png)

**In this screen, you can get Application (Client) ID**  
Click Redirect URIs

![image](https://user-images.githubusercontent.com/17783561/154855473-19713858-8c6e-49f0-ab13-51c33fc245fb.png)

Add 'Msal Only'

![image](https://user-images.githubusercontent.com/17783561/154855535-6b1abe22-a310-4038-89ad-b5c1cc006327.png)

Scroll down and 'Allow public client flows'

![image](https://user-images.githubusercontent.com/17783561/154855569-e5e8441e-30ad-4930-af5d-5e790ad9e1ce.png)

Click 'Save' button


Done. use your Application (client) ID.