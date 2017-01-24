# MeesGame
The something and only game without a number!
This is gonna "git" gud

### Dependencies
 - For building the release with an installer, install [NSIS](http://nsis.sourceforge.net/Download) in `C:\Program Files (x86)\NSIS`

### How to work on the project:
 - Clone the repository
 - BRANCH OFF!!!
 - Work
 - Commit
 - Format using Ctrl+K Ctrl+D
 - Submit pull request
 - Wait for others to review and eventually merge it
 - If there are merge conflicts, (git shell) [git checkout master] git pull, [git checkout your branch] git rebase master -i

### Testing
The commit should satisfy the following conditions:
 - No unnecessary usings
 - No namespaces other than 'MeesGame', 'MeesGen', 'AI' or 'Editor', unless explicitly stated in the PR description
 - It must compile and run without crashing (no errors)
 - Stuff (code and images) that is no longer needed is removed
 - Code is properly structured, changes to old structures should be mentioned
