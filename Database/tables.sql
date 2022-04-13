CREATE TABLE [users] (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    email varchar(100) NOT NULL UNIQUE,
    username varchar(20) NOT NULL UNIQUE,
    password varchar(100) NOT NULL,
    created_at datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE [conversations] (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    first_participant INT NOT NULL REFERENCES [users] (id),
    second_participant INT NOT NULL REFERENCES [users] (id),
    last_message_id INT
);


CREATE TABLE [messages] (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    content varchar(200) NOT NULL,
    user_id INT NOT NULL REFERENCES users ON DELETE CASCADE,
    conversation_id INT NOT NULL REFERENCES conversations,
    created_at datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
);

