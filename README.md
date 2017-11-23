# PageRuleAdmin
Administration for [CloudFlare](https://www.cloudflare.com) page rules. This currently only support URL Forward page rules.

## Get Started
1. Clone the Repo
2. Set `CF_API_KEY` and `CF_EMAIL` environment variables to the appropriate values. You'll need a CloudFlare account if you don't already have one.
3. `dotnet run`
4. Navigate to http://localhost:5000
5. Enter in the domain name of the domain you'd like to administrate page rules for

## Why did I build this?
I make extensive use of CloudFlare's page rules for redirects for my side business. Allowing people to administrate the page rules while
not giving them full access to CloudFlare became rather important for me. This is built to serve a very narrow and specific purpose, but
the code could be adapted pretty easily to start handling additional page rule types, or additional CloudFlare services.
