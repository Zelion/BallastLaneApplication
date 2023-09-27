#Steps:
1 Execute 'docker-compose up -d' on the Solution path to generate Images and Containers on Docker
2 Consume CreateAsync in the UserController (this will create a user in the Users Collection in MongoDB)
3 Consume Login in the UserController (this will return a JWT token)
4 Consume AddAsync in the ProductController using the previously retrieved JWT token (this will create a product related to a user in the Products Collection in MongoDB)