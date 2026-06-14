Security and Secret-Handling Guide

This repository contained a committed secret. Follow these steps immediately to contain exposure and harden future workflows.

1) Immediate actions
- Revoke or rotate any exposed credentials immediately (ImgBb API keys, demo passwords, tokens).
- If the exposed key was used in production, assume compromise and rotate in all systems.

2) Remove secrets from the repository history
Option A — BFG Repo Cleaner (easier):
- Install BFG (https://rtyley.github.io/bfg-repo-cleaner/)
- Create a text file (secrets-to-delete.txt) with the exact secret values or file paths to remove.
- Run (PowerShell):
  java -jar bfg.jar --replace-text secrets-to-delete.txt
  git reflog expire --expire=now --all
  git gc --prune=now --aggressive
  git push --force

Option B — git filter-repo (recommended for advanced use):
- Install git-filter-repo (https://github.com/newren/git-filter-repo)
- Example (remove key occurrences):
  git filter-repo --replace-text replacements.txt
  # replacements.txt format: regex==>replacement (see docs)
- Force-push rewritten history:
  git push origin --force --all
  git push origin --force --tags

Notes:
- After history rewrite, inform collaborators to re-clone (old clones will reference removed history).
- Back up the repository before rewriting history.

3) Prevent future leaks
- Do NOT commit secrets to source. Add runtime files and IDE folders to .gitignore (this repo includes one).
- For local development use dotnet user-secrets (for secrets used by the app) or environment variables.
  dotnet user-secrets init --project QrEventApi.csproj
  dotnet user-secrets set "ImgBb:ApiKey" "<key>"
- For CI/CD and production use a secrets manager (GitHub Actions Secrets, Azure Key Vault, AWS Secrets Manager).
- Read secrets at runtime (configuration providers) and never hard-code them in appsettings.json.

4) Rotate and reissue credentials
- Issue new keys after revocation and update the systems that depend on them.
- Confirm that the old keys are unusable.

5) Verify
- Search the repo (and cloned forks) for any remaining occurrences of the secret values.
- Use a secrets detection tool (GitGuardian, truffleHog, gitleaks) to scan history and branches.

6) Helpful references
- BFG Repo Cleaner: https://rtyley.github.io/bfg-repo-cleaner/
- git-filter-repo: https://github.com/newren/git-filter-repo
- Microsoft: Protect secrets in .NET: https://learn.microsoft.com/dotnet/secure/secret-management

If you want I can also:
- Add a SECURITY.md to this repo (done), and update README with direct remediation commands.
- Prepare a small script to automate detection and safe removal suggestions.
