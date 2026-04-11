using System;
using System.Collections.Generic;

namespace CybersecurityBot
{
    public class Topic
    {
        public string Name { get; }
        public string WhatIs { get; }
        public string ShortDescription { get; }
        public string WhenWhere { get; }
        public string Prevention { get; }
        public string Reference { get; }

        public Topic(string name, string whatIs, string shortDesc,
                     string whenWhere, string prevention, string reference)
        { Name = name; WhatIs = whatIs; ShortDescription = shortDesc; WhenWhere = whenWhere; Prevention = prevention; Reference = reference; }
    }

    public class Scenario
    {
        public string Description { get; }
        public List<string> Options { get; }
        public int CorrectOptionIndex { get; }
        public string Feedback { get; }

        public Scenario(string description, List<string> options, int correctIndex, string feedback)
        { Description = description; Options = options; CorrectOptionIndex = correctIndex; Feedback = feedback; }
    }

    public class Bot
    {
        private readonly Random _rand = new();
        public List<Topic> Topics { get; }
        private readonly List<(string Fact, string Citation)> _facts;
        private readonly List<Scenario> _scenarios;

        public Bot() { Topics = InitTopics(); _facts = InitFacts(); _scenarios = InitScenarios(); }

        public string GetRandomTip() => $"💡 TIP: {Topics[_rand.Next(Topics.Count)].ShortDescription}";
        public Scenario GetRandomScenario() => _scenarios[_rand.Next(_scenarios.Count)];
        public (string Fact, string Citation) GetRandomFact() => _facts[_rand.Next(_facts.Count)];

        public Topic FindTopic(string name)
        {
            foreach (var t in Topics)
                if (t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) return t;
            return null;
        }

        public string BuildTopicResponse(Topic t) =>
            $"📘 {t.Name.ToUpper()}\n" +
            $"{"━".PadRight(62, '━')}\n" +
            $"❓ WHAT IS {t.Name.ToUpper()}?\n   {t.WhatIs}\n\n" +
            $"⚠️  WHY IT MATTERS:\n   {t.ShortDescription}\n\n" +
            $"📍 WHEN & WHERE YOU'RE AT RISK:\n   {t.WhenWhere}\n\n" +
            $"🛡️  HOW TO PROTECT YOURSELF:\n   {t.Prevention}\n\n" +
            $"📖 Source: {t.Reference}\n" +
            $"{"━".PadRight(62, '━')}";

        public string GetEncouragement()
        {
            string[] msgs = {
                "You're doing great! Every question makes you safer online.",
                "That's a smart thing to ask. Stay curious!",
                "I'm proud of you for learning this. It really matters."
            };
            return msgs[_rand.Next(msgs.Length)];
        }

        // ✅ FIX: This method was called in Conversation.cs but was missing from Bot.cs
        public string GetComfortMessage()
        {
            string[] msgs = {
                "No worries at all! Learning takes time. You're doing brilliantly. 😊",
                "Don't be hard on yourself – even experts make mistakes. Let's keep going! 💪",
                "That's okay! The important thing is that you're here and learning. 🌟"
            };
            return msgs[_rand.Next(msgs.Length)];
        }

        // ---- Data ----

        private List<Topic> InitTopics() => new()
        {
            new("Passwords",
                "A password is a secret word or phrase that proves who you are – like the key to your digital front door.",
                "Weak or reused passwords are the easiest way for attackers to break into your accounts.",
                "When you create new accounts or reuse the same password across email, banking, and social media.",
                "• Use a unique 12+ character password per account.\n• Use a password manager (e.g., Bitwarden).\n• Enable 2FA on every account that supports it.",
                "(Verizon DBIR, 2023); (Pieterse, 2021)"),

            new("Phishing",
                "Phishing is when a criminal sends a fake message pretending to be a real company to steal your credentials.",
                "Fake messages trick you into handing over your login details or downloading a virus.",
                "Via email, SMS ('smishing'), or social media – often disguised as your bank, SARS, or a courier.",
                "• Check the sender's email carefully for misspellings.\n• Hover over links before clicking.\n• Never share your OTP, PIN, or password – banks never ask via email.",
                "(SABRIC, 2024); (Pieterse, 2021)"),

            new("Safe Browsing",
                "Safe browsing means visiting websites in a way that prevents criminals from stealing your information.",
                "Dangerous websites can silently install malware or steal session cookies without you knowing.",
                "When visiting sites without HTTPS, clicking pop-ups, or using shortened links.",
                "• Look for 🔒 and 'https://' before entering personal info.\n• Avoid downloading files from unknown sites.\n• Use uBlock Origin to block dangerous pop-ups.",
                "(Google Safe Browsing Report, 2023); (Pieterse, 2021)"),

            new("Two-Factor Authentication (2FA)",
                "2FA adds a second login step – something you have (a code) on top of something you know (your password).",
                "Even if a criminal steals your password, they can't get in without the second factor.",
                "Supported by Gmail, WhatsApp, Instagram, Capitec, FNB, and most banking apps.",
                "• Enable 2FA everywhere it's offered.\n• Use an authenticator app (Google/Microsoft Authenticator) over SMS.\n• Save backup codes in a safe place.",
                "(NIST SP 800-63B, 2020); (Pieterse, 2021)"),

            new("Malware",
                "Malware is malicious software – viruses, ransomware (locks your files), and spyware (steals passwords).",
                "Malware can encrypt files, spy on keystrokes, or turn your device into an attack tool.",
                "Spread via infected email attachments, compromised websites, fake downloads, and USB drives.",
                "• Keep all software updated.\n• Use reputable antivirus (Kaspersky, Bitdefender, Windows Defender).\n• Back up files to external drives or cloud storage regularly.",
                "(Kaspersky Security Bulletin, 2023); (Pieterse, 2021)"),

            new("Social Engineering",
                "Social engineering is tricking YOU into giving away information instead of hacking a computer directly.",
                "Attackers manipulate people – not machines – to reveal passwords or OTPs.",
                "Phone calls (vishing), email (phishing), and in-person impersonation (e.g., fake IT support).",
                "• Never give sensitive info unless you initiated the contact.\n• Hang up and call back on the official number from your bank card.\n• Be suspicious of any request for your password, OTP, or PIN.",
                "(Hadnagy, 2018); (SABRIC, 2024)"),

            new("Software Updates",
                "Software updates fix known security holes (vulnerabilities) in your OS, browser, or apps – for free.",
                "Unpatched software is the most common way criminals break into home and business computers.",
                "Every device – phone, laptop, router, even smart TV – should update regularly.",
                "• Enable automatic updates for Windows, macOS, Android, and iOS.\n• Restart when updates are ready – don't postpone indefinitely.\n• Never ignore update notifications; they often fix serious security bugs.",
                "(CISA, 2023); (Pieterse, 2021)"),

            new("Public Wi-Fi",
                "Public Wi-Fi is a wireless network in a coffee shop, airport, or hotel – often open and unencrypted.",
                "On an open network, an attacker can intercept everything you send – passwords, emails, banking details.",
                "When connecting to password-free Wi-Fi at coffee shops, airports, or shopping malls.",
                "• Avoid sensitive activities (banking, email) on public Wi-Fi.\n• Use a trusted VPN (NordVPN, ProtonVPN) if you must connect.\n• Ask staff for the exact network name – criminals set up fake hotspots.",
                "(SANS Institute, 2022); (Pieterse, 2021)")
        };

        private List<(string, string)> InitFacts() => new()
        {
            ("Phishing attacks targeting South African users increased by 63% in 2023.", "(SABRIC, 2024)"),
            ("Ransomware costs SA businesses an average of R2.5 million per incident.", "(Kaspersky, 2023)"),
            ("Only 38% of South Africans use a password manager.", "(Pieterse, 2021)"),
            ("80% of data breaches globally involve weak or stolen passwords.", "(Verizon DBIR, 2023)"),
            ("The banking sector saw a 42% rise in social engineering scams in 2024.", "(SABRIC, 2024)"),
            ("South Africa ranks 3rd in Africa for cybercrime activity.", "(Interpol AACP, 2023)")
        };

        private List<Scenario> InitScenarios() => new()
        {
            new("📧 You receive an email from your bank asking you to click a link and verify your account urgently. The sender looks real but the link goes to a suspicious domain.",
                new() { "Click the link – it sounds urgent.", "Call your bank using the number on the back of your card.", "Forward the email to a friend to check." },
                1, "Real banks never ask you to verify accounts via unsolicited email links. Always call the official number. (Pieterse, 2021)"),

            new("☕ You're at a coffee shop and see a Wi-Fi network named 'Free_Coffee_WiFi' – no password required.",
                new() { "Connect and quickly check your bank balance.", "Connect but avoid logging into sensitive accounts.", "Don't connect – it could be a rogue hotspot." },
                2, "Open Wi-Fi is often unencrypted. A hacker can intercept everything you send. Use a VPN if you must connect. (SANS, 2022)"),

            new("🖥️ A browser pop-up says 'Your PC is infected! Call 0800-123-456 now for free support.'",
                new() { "Call the number immediately.", "Close the browser tab and run a trusted antivirus scan.", "Pay the fee to unlock your files." },
                1, "This is a classic tech-support scam. Legitimate security software never demands you call a phone number via a pop-up. (FTC, 2023)"),

            new("📱 You get a WhatsApp from an unknown number: 'Mom/Dad, it's me – I broke my phone. Please send R2000 to this account.'",
                new() { "Send the money – it might really be your child.", "Call your child on their real phone number to check.", "Reply asking for a photo to prove it's them." },
                1, "This is a common impersonation scam. Always verify by calling the person directly on the number you know. (SABRIC, 2024)"),

            new("💼 You receive an email offering a work-from-home job paying R5000/week. They ask for a R500 'registration fee' and your ID number.",
                new() { "Pay the fee – it's a small price for a good job.", "Ignore it – real jobs never ask you to pay to get hired.", "Send your ID but not the money." },
                1, "This is an advance-fee scam. Legitimate employers never ask for money upfront. (Pieterse, 2021)")
        };
    }
}
