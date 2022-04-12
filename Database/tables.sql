CREATE TABLE [users] (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    email varchar(100) NOT NULL UNIQUE,
    username varchar(20) NOT NULL UNIQUE,
    password varchar(100) NOT NULL,
    created_at datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE [conversations] (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    last_message_id INT
);

CREATE TABLE [participants] (
    user_id INT NOT NULL REFERENCES users ON DELETE CASCADE,
    conversation_id INT NOT NULL REFERENCES conversations ON DELETE CASCADE,
    PRIMARY KEY (user_id, conversation_id)
);


CREATE TABLE [messages] (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    content varchar(200) NOT NULL,
    user_id INT NOT NULL REFERENCES users ON DELETE CASCADE,
    conversation_id INT NOT NULL REFERENCES conversations,
    created_at datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
);







ALTER TABLE dbo.participants
    ADD CONSTRAINT [FK_participants_conversation_id] 
FOREIGN KEY ([conversation_id]) REFERENCES dbo.[conversations] ([id]);