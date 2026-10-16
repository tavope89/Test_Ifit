# ITERATION_PLAYBOOK — Efficient, Safe Development

> **Applies to:** future implementations, fixes, releases and incident responses in this repository.
> **Goal:** avoid repeated SSH/PR iterations without sacrificing safety, maintainability or evidence.
> **Priority:** this is a workflow guide, not a change in project scope or an execution authorization. Root `AGENTS.md`, existing project contracts/runbooks and user-approved requirements govern. More restrictive security rules always win.
> **Template origin:** lessons from Hermes H3 Remote Operations, Media Optimizer, IPTV/Bazarr and other homelab deployments. Adapt the checks below to the actual repository; do not copy domain-specific implementations.

## 1. Read first; never rely solely on chat history

Before beginning, read `AGENTS.md`, this document, project README/runbooks and any current implementation/status files. Inspect **current remote default branch** (`main` or `master`), open PRs/other active branches and the exact SHA. Distinguish **implemented in source**, **CI PASS**, **merged**, **deployed**, **live verified** and **accepted**. If production evidence is missing, say UNKNOWN. Preserve other collaborators' work; never assume a previous chat's SHA is current.

State one exact acceptance target, the smallest safe next step and the operations expressly authorized. Keep follow-ups out of scope unless the user approves an extension.

## 2. A complete read-only diagnosis before the first fix

Write likely root-cause hypotheses and collect the smallest set of **discriminating evidence** together. Prefer one bounded, version-controlled preflight rather than an SSH ping-pong of one command per round.

As relevant: Git SHA and worktree cleanliness, deployed revision, runtime service versions, Compose overlays, health, dependency connectivity/DNS, container vs image identity, storage mounts and volume types, network/IPAM routes, relevant scheduler/worker state, persistent state and active jobs.

Use bounded, **sanitized** output: `PASS/FAIL/UNKNOWN`, precise component, category, observed/expected versions and next blocker. Do not paste raw Docker inspect output, full logs, secrets, tokens, env files, customer data or private paths into issues/chats.

Separate *test/probe defect* from *service defect*, *environment mismatch*, *partial prior deployment* and *unrelated incident*. Reproduce the concrete failure before changing production. If information is sufficient, act; do not demand another diagnostic cycle just to reconfirm unchanged evidence.

## 3. Fix the minimum responsible layer

Prefer established architecture and reuse existing modules/APIs/data over new frameworks, services, databases or duplicated pipelines. Make the smallest coherent patch with a regression test. Avoid refactors, extra UI, new features, opportunistic cleanup or cross-project edits unrelated to the stated goal.

When a defect is discovered **before** the current PR merges, fix it on that same scoped PR when safe. Do not manufacture a PR for every line. Separate independent risk boundaries or scopes into separately reviewable PRs; do not combine unrelated phases.

Preserve original product invariants and user-approved acceptance criteria. Report a true scope change before implementing it.

## 4. Tests proportional to change and risk

- **Documentation only:** verify links, references, file scope, exact diff; never rebuild/restart production for prose changes.
- **Pure logic/contract:** focused unit/typing/lint + characterization/negative tests.
- **API, integration or async jobs:** contract and mock/disposable integration tests, idempotency, replay, failure paths and accurate events/results.
- **Docker/network/privileges/config:** exact merged Compose config, least-privilege checks, IPAM/route conflicts, changed service dependencies and target health.
- **Database/storage/destructive changes:** disposable migration/rollback and state invariants; separately authorized backup/restore and apply-once production procedure.
- **Host mutations and deployment:** explicit authorization, exact target/SHA, verified preconditions, targeted change, postconditions, unaffected-service identity and rollback plan.

Re-run the original failing check plus affected regressions. Run broader/full suites when a release gate requires them or when impacted code/config invalidates prior evidence. Do not claim a CI result validates a real running host.

## 5. Two-pass release; no blind replay

**Stage:** build/test at pinned commit, diff the exact Compose/config/permissions to be deployed, verify credential *presence* without revealing values, and prepare target-only apply, rollback and post-checks. Production is not a disposable test environment.

**Apply:** after approval where required, recheck source and runtime preconditions; operate on only the named services/resources, preserve all unrelated containers/networks/media/jobs, and measure real functional outcomes. Never use global `docker compose down/up`, uncontrolled prune, force-push/reset or broad restarts as a shortcut.

On partial failure, identify the **durable commit point** (e.g. applied migration, changed file, restarted container), **never rerun** a one-shot installer/reset by habit, and create a reviewed resume-only procedure for pending steps. Fail closed if unexpected drift appears.

Local operator scripts do not become implicitly authorized remote automation. Read-only inspection and mutations must be visibly separate.

## 6. Git collaboration and delivery

Use a dedicated branch/worktree based on the **current** default branch; do not edit production checkout directly. Check branch vs current remote main/master before merge and again before deploy. Use expected-head SHA guards and compatible strategy; no force operations, discarding uncommitted work or overwriting another PR.

Keep one coherent, reviewable PR with useful code, tests and documentation; no cosmetic churn. Respect `.gitignore`, ensure files required at build time are actually tracked, and verify image/build provenance rather than assuming source==runtime.

No secrets in Git, comments or test fixtures. Do not invent credentials or success evidence. Prefer a short reproducible runbook/script over a long unreviewed shell pasted in chat.

## 7. Repeat-failure prevention: hard lessons

| Previous failure class | Mandatory preventive step |
| --- | --- |
| Docker image falsely identified as existing container | Use **typed** `docker container inspect`, `docker image inspect`, `docker network inspect` as appropriate; regression-test type ambiguity |
| Docker IPAM/address pools exhausted | Inventory existing Docker networks, host routes, interface CIDRs and LAN/VPN conflicts *before* creating a new network |
| OCI failure linked to mount/path | Inspect actual mount source type and Compose-resolved bind before guessing from exit code or restarting |
| Probe contradicts canonical policy | Reuse policy exceptions, normalized Docker capabilities and resolved network names; do not loosen audit to pass |
| Docker runtime version differs from desired Compose version | Distinguish before/after states in migration/upgrade preflight |
| App settings appear valid but network access fails | Test real container DNS, network membership, mounts and authenticated connectivity |
| A deployment partially committed migration or deletion | Verify durable state and **resume incomplete steps only**; never blindly replay |
| Host directory differs from Git checkout | Verify real systemd/Compose working directory, deployment layout and source staging |
| Build worked locally but source files absent from Git | Audit tracked source, ignore patterns and build context before image build |
| Many tiny PR/SSH loops | Batch safe diagnosis and closely related fixes/tests, while preserving approval and scope gates |
| Concurrent branches/media workers | Inspect active workers/PRs/jobs, reserve resources and keep unaffected services/data untouched |

These are patterns, **not commands to change** unrelated systems. Use only the checks relevant to this project.

## 8. Status, handoff and definition of done

At the end of an iteration record, compactly: phase/task, exact base and candidate SHA, PR, tests and host evidence, deployment SHA, true state (PASS/FAIL/BLOCKED/UNKNOWN), changed resources, preserved unrelated state and the **single next safe action**.

Prefer a short append-only status entry where a status file exists. Do not replace earlier evidence, hide failures or claim work is completed based on a merged PR or a running container alone. Prove the actual user-visible acceptance criteria.

**Efficient = correctly completed or explicitly blocked with evidence, within scope, with minimal unnecessary handoffs. Safety, authorization and data preservation outrank speed.**

## 9. How to use this for a new implementation

1. Read root `AGENTS.md`, this file and the current project-specific rules/status.
2. Define a minimal end-to-end vertical slice and explicit acceptance checks.
3. Reuse existing working infrastructure; design targeted tests and a release/rollback boundary.
4. Implement, verify, review and release in the smallest safe complete increment.
5. Record the source/runtime/evidence state so the next chat never starts the investigation from zero.

## 10. Repository-specific checks — Test_Ifit

- Legacy Visual Studio test/application code. Use actual solution/project build and test tooling, not unrelated Docker deployment recipes.
- Identify dependency and environment versions before fixes, preserve older compatibility constraints and avoid speculative modernizations.
- No live hardware/device operations or data changes without separate explicit authorization.
