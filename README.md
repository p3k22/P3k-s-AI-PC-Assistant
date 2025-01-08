-Work In Progress-

- Uses ChatGPT for chat interactions.
- Uses Open AI and Goodle Text To Speech services.
- Uses Goodle Speech To Text services.

WPF is incomplete and in an unfinished state currently.

# The IDEA
To have an integrated windows app that runs in the background of a users PC and can interact with the app via talking or typing to it.
*Hopeful Features*
- Quick file search and open via voice command
- Internet browsal requests such as visiting a site and searching for something
- (Rick's Garage type AI). Voice recording, recognition, playback, sarcasm! "AI gonna AI"

# The Framework
I have tried to split the framework of this project into 2 solutions (ServerHost / Client) containing multiple projects.

The WPF will interact with all the smaller projects, such as ChatGPT chat request/responces and TTS services. 

These projects can be used independently in other project without depending on anything else.

The user interacts with the application and all interactions are sent via http to my hosted server with each user having their own API keys. 

The server will make the required prompts for the user and stream the responses back to the user, allowing for lightning fast responses from the GPT model.

Streaming responses this way allows conversation to flow naturally, using both OpenAI's and Googles TTS services.
