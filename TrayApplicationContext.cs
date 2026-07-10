using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkyBlockTray;

public class TrayApplicationContext : ApplicationContext
{
    private readonly Icon greenIcon;
    private readonly Icon redIcon;
    private readonly Icon yellowIcon;

    private readonly NotifyIcon trayIcon;
    private readonly System.Threading.Timer timer;

    private bool? lastStatus = null;


    public TrayApplicationContext()
    {
        greenIcon = new Icon("Icons\\green.ico");
        redIcon = new Icon("Icons\\red.ico");
        yellowIcon = new Icon("Icons\\yellow.ico");


        trayIcon = new NotifyIcon()
        {
            Icon = yellowIcon,
            Visible = true,
            Text = "Hypixel SkyBlock: Checking..."
        };


        var menu = new ContextMenuStrip();

        menu.Items.Add(
            "Check Now",
            null,
            async (s, e) => await CheckStatus()
        );


        menu.Items.Add(
            "Exit",
            null,
            (s, e) => Exit()
        );


        trayIcon.ContextMenuStrip = menu;


        // First check immediately
        _ = CheckStatus();


        // Check every minute
        timer = new System.Threading.Timer(
            async _ => await CheckStatus(),
            null,
            TimeSpan.FromSeconds(60),
            TimeSpan.FromSeconds(60)
        );
    }



    private async Task CheckStatus()
    {
        int players = await SkyBlockStatusService.GetSkyBlockPlayerCount();


        bool online = players >= 25;


        if (players == -1)
        {
            trayIcon.Icon = yellowIcon;
            trayIcon.Text = "SkyBlock: Unable to check";
        }
        else if (online)
        {
            trayIcon.Icon = greenIcon;
            trayIcon.Text = $"SkyBlock ONLINE - {players:N0} players";
        }
        else
        {
            trayIcon.Icon = redIcon;
            trayIcon.Text = $"SkyBlock Maintenance - {players:N0} players";
        }



        // Only notify if the status changed
        if (lastStatus != null && lastStatus != online)
        {
            if (online)
            {
                NotificationService.Show(
                    "SkyBlock is Back!",
                    $"SkyBlock is online again ({players:N0} players)."
                );
            }
            
            
        }


        lastStatus = online;
    }



    private void Exit()
    {
        timer.Dispose();

        trayIcon.Visible = false;
        trayIcon.Dispose();

        greenIcon.Dispose();
        redIcon.Dispose();
        yellowIcon.Dispose();

        Application.Exit();
    }
}