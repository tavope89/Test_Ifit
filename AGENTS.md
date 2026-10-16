# AGENTS.md — Test_Ifit

> Instructions for AI contributors and future ChatGPT implementation sessions. Scope: this repository only.

## Mandatory reading and source of truth

1. Read **[docs/ITERATION_PLAYBOOK.md](docs/ITERATION_PLAYBOOK.md)** before planning, coding, operating or deploying any change.
2. Read existing README, contracts, runbooks, CI and current default-branch state. Project-specific user-approved plans and stricter rules take precedence over the playbook.
3. Inspect latest remote main/master SHA, outstanding PRs, working-tree changes, environment and actual deployed version. Never assume historical chat context proves current runtime status.
4. Define one exact acceptance goal and implement the smallest coherent, tested change. Avoid unrelated refactors, duplicate services, unnecessary PR/SSH cycles or broad restarts.
5. Apply risk-appropriate CI/host checks, pin reviewed SHA, preserve collaborators' changes and never repeat committed destructive steps. Production mutations require explicit authorization.
6. State separately SOURCE / CI / MERGED / DEPLOYED / VERIFIED and record next safe step. Do not claim success without evidence.

## Project boundary

Legacy Visual Studio solution; avoid unrelated refactors, model/hardware/device access or deployment assumptions.

The playbook improves **how** work is implemented; it never authorizes operations, changes the project's functional goals or relaxes data/security protections. No secrets in commits/logs/issues and no direct production modification without permission.
