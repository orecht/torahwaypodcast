# Infrastructures
## Domain name 
torahwaypodcast.org.uk is registered with 1and1. 

## DNS
Using 1and1 DNS server. 
Best setting would be to have a CNAME of torahwaypodcast.org.uk to sample-env.5qnbyuhbvs.us-west-2.elasticbeanstalk.com. Unfortunately 1and1 doesn't allow CNAME for anything else than subdomains. 

Next best solution would be to use a different DNS provider. CouldFlare is supposed to be free. Could use Route 53 but no free tier.

Single-instance domains have they elastic IP lined to the instance. So solution is to add an A record in 1and1 DNS to 35.166.189.50 for torahwaypodcast.org.uk

More info on EPI: http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/elastic-ip-addresses-eip.html

## Hosting
Site is hosted on AWS. 
http://sample-env.5qnbyuhbvs.us-west-2.elasticbeanstalk.com/
Currently on Oregon region. Consider moving to Ireland. 

Note: elastic IP (EIP) depends on the region. 

AWS free tier. opened on 20/12/2016. WIll expire 12 months later. 

http://torahwaypodcast.org.uk/home/parsehtml is called every 15 min by a AWS lambda function

# Operations
* Feed is at http://torahwaypodcast.org.uk/home/rss2 (default document for http://torahwaypodcast.org.uk/)
* parsing is processed when http://torahwaypodcast.org.uk/home/parsehtml is called.

# Dev environment:
* ASP.NET MVC 4
* .NET 4.5 (4.0 would be enough, not using async)


# Ideas for future delvelopment:
Data persistence. 
 * <s>Everything is loaded in memory. Then lost when IIS appl pool is recycled, which probably happens very often (3 min) in cloud environment.</s>
 * Don't need to use database for persistance at the moment. only 1 table. Might still be best for performance. 

Feed for Edgware branch. (Gatshead sire not updated since 2013)
* Looks like same HTML template. SHould be easy. 

Feed for Manchester 
* Completely different HTML. Need to write a different parser.

<s>Add image to feed</s> DONE

Add mugshot of the speaker taken from the weekly poster
* use Azure Cognituve API to detect faces?
* How to match face with shiur? 